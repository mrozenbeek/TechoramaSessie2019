using Client.Constants;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Helpers
{
    public static class HttpClientHelper
    {


        private static async Task<TokenResponse> GetTokenResponse()
        {
            DiscoveryResponse disco = await DiscoveryClient.GetAsync(ClientConstants.AuthorityUrl);
            
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                throw new Exception(disco.Error);
            }

            // request token
            TokenClient tokenClient = new TokenClient(disco.TokenEndpoint, ClientConstants.ClientId, ClientConstants.ClientSecret);   //nOA4T294yU0AJxygG0QCs4rFlp89dD79EavyFJ6dka8BODX5AjE5vWBplb+HKCojJWrhBAY44k/xLq4mQ1UKWA==
            TokenResponse tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                throw new Exception(tokenResponse.Error);
            }

            return tokenResponse;

            //Console.WriteLine(tokenResponse.Json);
        }

        public static async Task<string> GetContentsFromApi()
        {
            string content = string.Empty;
            TokenResponse tokenResponse = await GetTokenResponse();

            HttpClient client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            HttpResponseMessage response = await client.GetAsync($"{ClientConstants.APIUrl}identity");
            if (!response.IsSuccessStatusCode)
            {
                //Console.WriteLine(response.StatusCode);
                throw new Exception(response.StatusCode.ToString());
            }
            else
            {
                content = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(JArray.Parse(content));
            }
            return content;

        }

    }
}
