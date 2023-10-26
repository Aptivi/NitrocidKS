﻿//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KS.Files;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.Shells.HTTP;

namespace KS.Network.HTTP
{
    /// <summary>
    /// HTTP tools
    /// </summary>
    public static class HTTPTools
    {

        /// <summary>
        /// Deletes the specified content from HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetodelete.html")</param>
        public async static Task HttpDelete(string ContentUri)
        {
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            await ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).DeleteAsync(TargetUri);
        }

        /// <summary>
        /// Gets the specified content string from HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        public async static Task<string> HttpGetString(string ContentUri)
        {
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            return await ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).GetStringAsync(TargetUri);
        }

        /// <summary>
        /// Gets the specified content from HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        public async static Task<HttpResponseMessage> HttpGet(string ContentUri)
        {
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            return await ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).GetAsync(TargetUri);
        }

        /// <summary>
        /// Puts the specified content string to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentString">String to put to the HTTP server</param>
        public async static Task<HttpResponseMessage> HttpPutString(string ContentUri, string ContentString)
        {
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            var stringContent = new StringContent(ContentString);
            return await ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).PutAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Puts the specified file to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentPath">Path to the file to open a stream and put it to the HTTP server</param>
        public async static Task<HttpResponseMessage> HttpPutFile(string ContentUri, string ContentPath)
        {
            ContentPath = FilesystemTools.NeutralizePath(ContentPath);
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            var TargetStream = new FileStream(ContentPath, FileMode.Open, FileAccess.Read);
            var stringContent = new StreamContent(TargetStream);
            return await ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).PutAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Posts the specified content string to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentString">String to post to the HTTP server</param>
        public async static Task<HttpResponseMessage> HttpPostString(string ContentUri, string ContentString)
        {
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            var stringContent = new StringContent(ContentString);
            return await ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).PostAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Posts the specified file to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentPath">Path to the file to open a stream and post it to the HTTP server</param>
        public async static Task<HttpResponseMessage> HttpPostFile(string ContentUri, string ContentPath)
        {
            ContentPath = FilesystemTools.NeutralizePath(ContentPath);
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            var TargetStream = new FileStream(ContentPath, FileMode.Open, FileAccess.Read);
            var stringContent = new StreamContent(TargetStream);
            return await ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).PostAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Adds a request header to the future requests
        /// </summary>
        /// <param name="key">Key to assign a value to</param>
        /// <param name="value">Value to assign to this key</param>
        public static void HttpAddHeader(string key, string value)
        {
            if (!HttpHeaderExists(key))
                ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).DefaultRequestHeaders.Add(key, value);
            else
                throw new KernelException(KernelExceptionType.HTTPNetwork, Translate.DoTranslation("Adding header that already exists."));
        }

        /// <summary>
        /// Adds a request header to the future requests
        /// </summary>
        /// <param name="key">Key to remove</param>
        public static void HttpRemoveHeader(string key)
        {
            if (HttpHeaderExists(key))
                ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).DefaultRequestHeaders.Remove(key);
            else
                throw new KernelException(KernelExceptionType.HTTPNetwork, Translate.DoTranslation("Removing header that doesn't exist."));
        }

        /// <summary>
        /// Modifies a request header key for the future requests
        /// </summary>
        /// <param name="key">Key to assign a value to</param>
        /// <param name="value">Value to assign to this key</param>
        public static void HttpEditHeader(string key, string value)
        {
            if (HttpHeaderExists(key))
            {
                // We can't just index a key from the request header collection and expect it to set to another value. We need to
                // remove the key and re-add the same key with different value
                HttpRemoveHeader(key);
                HttpAddHeader(key, value);
            }
            else
                throw new KernelException(KernelExceptionType.HTTPNetwork, Translate.DoTranslation("Editing header that doesn't exist."));
        }

        /// <summary>
        /// Makes a list of headers
        /// </summary>
        /// <returns>An array of tuples containing keys and values from the HTTP request headers</returns>
        public static (string, string)[] HttpListHeaders()
        {
            var headers = ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).DefaultRequestHeaders;
            var finalHeaders = new List<(string, string)>();

            // Enumerate through headers to convert them to tuples
            foreach (var header in headers)
            {
                var values = header.Value;
                foreach (var value in values)
                    finalHeaders.Add((header.Key, value));
            }
            return finalHeaders.ToArray();
        }

        /// <summary>
        /// Checks to see if the specified key from the header exists
        /// </summary>
        /// <param name="key">Key to query</param>
        /// <returns>True if found; false otherwise.</returns>
        public static bool HttpHeaderExists(string key) =>
            ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).DefaultRequestHeaders.Contains(key);

        /// <summary>
        /// Gets the current user agent
        /// </summary>
        /// <returns>
        /// The current user agent. If there are two or more user agents set in the same header (by somehow adding the same
        /// key with different UA), returns the last user agent value.
        /// </returns>
        public static string HttpGetCurrentUserAgent()
        {
            var userAgents = ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).DefaultRequestHeaders.UserAgent;
            if (userAgents.Count > 0)
                // We don't support more than one UserAgent value, so return the last one and ignore the rest
                return userAgents.ElementAt(userAgents.Count - 1).ToString();
            return "";
        }

        /// <summary>
        /// Sets the current user agent
        /// </summary>
        /// <param name="userAgent">Target user agent</param>
        public static void HttpSetUserAgent(string userAgent)
        {
            // Remove all user agent strings in case we have more than one instance
            while (HttpHeaderExists("User-Agent"))
                HttpRemoveHeader("User-Agent");

            // Now, set the user agent
            HttpAddHeader("User-Agent", userAgent);
        }

        /// <summary>
        /// Neutralize the URI so the host name, <see cref="HTTPShellCommon.HTTPSite"/>, doesn't appear twice.
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        public static string NeutralizeUri(string ContentUri)
        {
            string NeutralizedUri = "";
            if (!ContentUri.StartsWith(HTTPShellCommon.HTTPSite))
                NeutralizedUri += HTTPShellCommon.HTTPSite;
            NeutralizedUri += ContentUri;
            return NeutralizedUri;
        }

    }
}
