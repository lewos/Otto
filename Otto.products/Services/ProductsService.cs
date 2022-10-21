using Otto.models;
using Otto.models.Responses;
using Otto.products.DTO;

namespace Otto.products.Services
{
    public class ProductsService
    {
        private readonly AccessTokenService _accessTokenService;
        private readonly MercadolibreService _mercadolibreService;
        private readonly TiendanubeService _tiendanubeService;
        private readonly StockService _stockService;

        public ProductsService(AccessTokenService accessTokenService, MercadolibreService mercadolibreService, TiendanubeService tiendanubeService, StockService stockService)
        {
            _accessTokenService = accessTokenService;
            _mercadolibreService = mercadolibreService;
            _tiendanubeService = tiendanubeService;
            _stockService = stockService;
        }

        public async Task<List<ProductDTO>> GetProductsByMUserId(string id)
        {
            var result = new List<ProductDTO>();

            var accessToken = await GetAccessToken(long.Parse(id));
            if(accessToken != null)
                return result;


            var mItemsSearchResponse = await _mercadolibreService.GetMItemsBySelledIdAsync(long.Parse(id),accessToken.AccessToken);
            if (mItemsSearchResponse.res != ResponseCode.OK)
                return result;


            var items = mItemsSearchResponse.itemsSearch.Results;

            var concatItems = string.Join(',', items);

            var mProductsSearchResponse = await _mercadolibreService.GetMProductsByItemsAsync(concatItems, accessToken.AccessToken);
            if (mProductsSearchResponse.res != ResponseCode.OK)
                return result;
            
            var itemsSegunMercadolibre = mProductsSearchResponse.mProductsSearch;

            // ahora tengo que chequear si ya tengo algo de eso en el deposito/stock
            var stockResponse = await _stockService.GetStockByMUserIdAsync(id);
            if (stockResponse.res != ResponseCode.OK)
                return result;
            
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

            return result;
        }



        public async Task<List<TProductDTO>> GetProductsByTUserId(string id) 
        {
            var result = new List<TProductDTO>();

            var accessToken = await GetAccessToken(long.Parse(id), true);
            if (accessToken == null)
                return result;

            var itemsSearchResponse = await _tiendanubeService.GetTItemsBySelledIdAsync(long.Parse(id), accessToken.AccessToken);

            if (itemsSearchResponse.res != ResponseCode.OK)
                return result;


            var itemsSegunTiendanube = itemsSearchResponse.itemsSearch.Items;

            // ahora tengo que chequear si ya tengo algo de eso en el deposito/stock
            var stockResponse = await _stockService.GetStockByTUserIdAsync(id);
            if (stockResponse.res != ResponseCode.OK)
                return result;

            var stockActualDelUsuario = stockResponse.stockDTOs;
            foreach (var itemSegunTiendanube in itemsSegunTiendanube)
            {

                // me fijo si existe en el stockActualDelUsuario ese item
                var stockActualDeUnItem = stockActualDelUsuario.Find(s => s.TItemId == itemSegunTiendanube.Id.ToString());
                if (stockActualDeUnItem != null)
                {
                    result.Add(new TProductDTO(itemSegunTiendanube, true, stockActualDeUnItem.Quantity));
                }
                else
                {
                    result.Add(new TProductDTO(itemSegunTiendanube, false, 0));
                }                               
            }
            return result;
        }

        private async Task<Token> GetAccessToken(long channelSellerId, bool isTiendanube = false)
        {
            AccessTokenResponse<Token> res = await _accessTokenService.GetTokenCacheAsync(channelSellerId, isTiendanube);

            // los access token de tiendanube no expiran
            if (!isTiendanube && hasTokenExpired(res.token))
                res = await _accessTokenService.GetTokenAfterRefresh(channelSellerId);
            return res.token;
        }
        private bool hasTokenExpired(Token token)
        {
            var utcNow = DateTime.UtcNow;
            // Si expiro o si esta a punto de expirar
            if (token.ExpiresAt < utcNow + TimeSpan.FromMinutes(10))
                return true;
            return false;
        }
    }
}
