using Otto.models;
using Otto.products.DTO;

namespace Otto.products.Services
{
    public class ProductsService
    {
        private readonly AccessTokenService _accessTokenService;
        private readonly MercadolibreService _mercadolibreService;
        private readonly StockService _stockService;

        public ProductsService(AccessTokenService accessTokenService, MercadolibreService mercadolibreService, StockService stockService)
        {
            _accessTokenService = accessTokenService;
            _mercadolibreService = mercadolibreService;
            _stockService = stockService;
        }

        public async Task<List<ProductDTO>> GetProductsByMUserId(string id)
        {

            var result = new List<ProductDTO>();

            var accessToken = await GetAccessToken(long.Parse(id));
            if (accessToken != null) 
            {
                var mItemsSearchResponse = await _mercadolibreService.GetMItemsBySelledIdAsync(long.Parse(id),accessToken.AccessToken);
                if (mItemsSearchResponse.res == ResponseCode.OK) 
                {
                    var items = mItemsSearchResponse.mItemsSearch.Results;

                    var concatItems = string.Join(',', items);

                    var mProductsSearchResponse = await _mercadolibreService.GetMProductsByItemsAsync(concatItems, accessToken.AccessToken);
                    if (mProductsSearchResponse.res == ResponseCode.OK) 
                    {
                        var itemsSegunMercadolibre = mProductsSearchResponse.mProductsSearch;

                        // ahora tengo que chequear si ya tengo algo de eso en el deposito/stock

                        var stockResponse = await _stockService.GetStockByMUserIdAsync(long.Parse(id));

                        if (stockResponse.res == ResponseCode.OK) 
                        {
                            var stockActualDelUsuario = stockResponse.stockDTOs;

                            foreach (var itemSegunMercadolibre in itemsSegunMercadolibre)
                            {
                                if (itemSegunMercadolibre.Code.Equals(200)) 
                                {
                                    // me fijo si existe en el stockActualDelUsuario ese item
                                    var stockActualDeUnItem = stockActualDelUsuario.Find(s => s.MItemId == itemSegunMercadolibre.Body.Id);
                                    if (stockActualDeUnItem != null)
                                    {
                                        result.Add(new ProductDTO(itemSegunMercadolibre.Body, true, stockActualDeUnItem.Quantity));
                                    }
                                    else 
                                    {
                                        result.Add(new ProductDTO(itemSegunMercadolibre.Body, false, 0));
                                    }
                                }
                                Console.WriteLine("No se obtuvo un codigo satisfactorio en la consulta del producto, ver log");    
                            }
                        }
                    }

                }
            }
            return result;
        }

        private async Task<MTokenDTO> GetAccessToken(long mUserId)
        {
            var res = await _accessTokenService.GetTokenCacheAsync(mUserId);

            if (hasTokenExpired(res.token))
                res = await _accessTokenService.GetTokenAfterRefresh(mUserId);
            return res.token;
        }

        private bool hasTokenExpired(MTokenDTO token)
        {
            var utcNow = DateTime.UtcNow;
            // Si expiro o si esta a punto de expirar
            if (token.ExpiresAt < utcNow + TimeSpan.FromMinutes(10))
                return true;
            return false;
        }
    }
}
