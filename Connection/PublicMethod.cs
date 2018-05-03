using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Omi.Education.Library.SignalR.Connection
{
    public class PublicMethod
    {
        public static string JsonSerialize<T>(T model) where T : class
        {
            try
            {
                return JsonConvert.SerializeObject(model);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public static T JsonDeSerialize<T>(string str) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public static string[] SplitString(string paths)
        {
            return paths.Split('&');
        }

        public static T TypeParse<T>(string str) where T : struct
        {
            T translation = (T)Enum.Parse(typeof(T), str);
            return translation;
        }

        public static Guid GetToken()
        {
            Guid guid = Guid.NewGuid();
            return guid;
        }

        public static string CombinationString(string hubToken ,string targetPath)
        {
            if (!String.IsNullOrEmpty(targetPath))
            {
                return hubToken + "&" + targetPath;
            }
            else
            {
                return hubToken;
            }
        }
    }
}