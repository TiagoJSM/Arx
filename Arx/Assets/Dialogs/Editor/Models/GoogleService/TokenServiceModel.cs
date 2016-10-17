using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * 
    "access_token": "ya29.Ci95Ax6ySwUcLUYLENte8Rp4oJzXj_KfKykH4Ddjf9TzW_KIJA6SrY_GhgYEQy4Zjg",
    "token_type": "Bearer",
    "expires_in": 3600,
    "refresh_token": "1/lIkMipHHRgA2cAWQHmcEIfkSCSbcz5-b7LA4rKTUtlg"
*
*/
[Serializable]
public class TokenServiceModel
{
    public string access_token;
    public string token_type;
    public int expires_in;
    public string refresh_token;
}
