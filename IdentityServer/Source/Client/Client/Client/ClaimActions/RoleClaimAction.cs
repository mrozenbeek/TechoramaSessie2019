using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.ClaimActions
{
    internal class RoleClaimAction : ClaimAction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RoleClaimAction()
            : base("role", ClaimValueTypes.String)
        {
        }

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="identity"></param>
        /// <param name="issuer"></param>
        public override void Run(JObject userData, ClaimsIdentity identity, string issuer)
        {
            var tokens = userData.SelectTokens("role");
            IEnumerable<string> roles;

            foreach (var token in tokens)
            {
                if (token is JArray)
                {
                    var jarray = token as JArray;
                    roles = jarray.Values<string>();
                }
                else
                    roles = new string[] { token.Value<string>() };

                foreach (var role in roles)
                {
                    Claim claim = new Claim("role", role, ValueType, issuer);
                    if (!identity.HasClaim(c => c.Subject == claim.Subject
                                             && c.Value == claim.Value))
                    {
                        identity.AddClaim(claim);
                    }
                }
            }
        }
    }
}
