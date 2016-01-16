/*
 * Copyright 2011 Witoslaw Koczewsi <wi@koczewski.de>
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the GNU Affero
 * General Public License as published by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the
 * implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public
 * License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License along with this program. If not,
 * see <http://www.gnu.org/licenses/>.
 */

/**
 * https://www.max-portal.elv.de/dwr/test/MaxRemoteApi
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using log4net;
using MaxControl.State;
using Newtonsoft.Json;

namespace MaxControl.Internet
{
    public class MaxSession
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(MaxSession));

        public String FloatToString(float value)
        {
            return value.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
        }

        public String BaseUrl { get; set; }

        public RequestExecutor RequestExecutor { get; set; }

        public int BatchId { get; set; }

        public String HttpSessionId { get; set; }

        public String ScriptSessionId { get; set; }

        public String User { get; set; }

        public String Password { get; set; }

        public MaxCubeState MaxCubeState { get; set; }

        private MaxSession(String baseUrl)
        {
            BaseUrl = baseUrl;

            if (!BaseUrl.EndsWith("/"))
                BaseUrl += "/";
        }

        public static MaxSession CreateElvInstance()
        {
            return new MaxSession("https://www.max-portal.elv.de/");
        }

        public static MaxSession CreateMdInstance()
        {
            return new MaxSession("https://smarthome.md.de/");
        }

        public static MaxSession CreateEq3Instance()
        {
            return new MaxSession("https://max.eq-3.de/");
        }

        public void ExecuteSetRoomAutoModeWithTemperature(int maxRoomId, float temperature)
        {
            var extra = new Dictionary<String, String>
            {
                {"c0-e2", "number:" + maxRoomId},
                {"c0-e3", "number:" + FloatToString(temperature)},
                {"c0-e1", "Object_MaxSetRoomAutoModeWithTemperature:{roomId:reference:c0-e2, temperature:reference:c0-e3}"}
            };
            ExecuteApiMethod("setClientCommands", extra, "Array:[reference:c0-e1]");
        }

        public void ExecuteSetRoomAutoMode(int maxRoomId)
        {
            var extra = new Dictionary<String, String>
            {
                {"c0-e2", "number:" + maxRoomId},
                {"c0-e1", "Object_MaxSetRoomAutoMode:{roomId:reference:c0-e2}"}
            };
            ExecuteApiMethod("setClientCommands", extra, "Array:[reference:c0-e1]");
        }

        public void ExecuteSetRoomEcoMode(int maxRoomId, float ecoTemperature)
        {
            ExecuteSetRoomPermanentMode(maxRoomId, ecoTemperature);
        }

        public void ExecuteSetRoomComfortMode(int maxRoomId, float comfortTemperature)
        {
            ExecuteSetRoomPermanentMode(maxRoomId, comfortTemperature);
        }

        public void ExecuteSetRoomPermanentMode(int maxRoomId, float temperature)
        {
            var extra = new Dictionary<String, String>
            {
                {"c0-e2", "number:" + maxRoomId},
                {"c0-e3", "number:" + FloatToString(temperature)},
                {"c0-e1", "Object_MaxSetRoomPermanentMode:{roomId:reference:c0-e2, temperature:reference:c0-e3}"}
            };
            ExecuteApiMethod("setClientCommands", extra, "Array:[reference:c0-e1]");
        }

        public void ExecuteSetRoomTemporaryMode(int maxRoomId, float temperature, DateTime untilDateTime)
        {
            if (untilDateTime == null)
                throw new ArgumentException("untilDateTime");

            var minute = untilDateTime.Minute;
            if (minute == 30 || minute == 0)
            {
                // no change
            }
            else if (minute > 27)
            {
                untilDateTime = untilDateTime.AddHours(1).AddMinutes(-untilDateTime.Minute);
            }
            else
            {
                untilDateTime = untilDateTime.AddMinutes(30 - untilDateTime.Minute);
            }

            var extra = new Dictionary<String, String>
            {
                {"c0-e2", "number:" + maxRoomId},
                {"c0-e3", "Date:" + untilDateTime},
                {"c0-e4", "number:" + FloatToString(temperature)},
                {
                    "c0-e1",
                    "Object_MaxSetRoomTemporaryMode:{roomId:reference:c0-e2, date:reference:c0-e3, temperature:reference:c0-e4}"
                }
            };
            ExecuteApiMethod("setClientCommands", extra, "Array:[reference:c0-e1]");
        }

        public MaxCubeState GetMaxCubeState()
        {
            var result = ExecuteApiMethod("getMaxCubeState", null);

            //if (!parser.contains("var s0=new MaxCubeState();"))
            //    throw new MaxProtocolException("Missing 'new MaxCubeState()' in response", result);

            MaxCubeState = (MaxCubeState)DwrParser.Parse(result);
            MaxCubeState.Wire();
            _log.Info("State loaded");
            return MaxCubeState;
        }

        void Relogin()
        {
            Login(User, Password);
        }

        public void Login(String user, String password)
        {
            initialize();

            var result = ExecuteApiMethod("login", null, "string:" + user, "string:" + password);

            //var parser = DwrParser.Parse(result);
            //if (parser.IsError()) throw new LoginFailedException(BaseUrl, user, parser.GetErrorMessage());

            //if (!parser.contains("dwr.engine._remoteHandleCallback("))
            //    throw new MaxProtocolException("Missing callback in response", result);

            User = user;
            Password = password;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private String ExecuteApiMethod(String name, Dictionary<String, String> extraParams, params String[] arguments)
        {
            BatchId++;

            var parameters = new Dictionary<String, String>
            {
                {"callCount", "1"},
                {"page", "/dwr/test/MaxRemoteApi"},
                {"httpSessionId", HttpSessionId},
                {"scriptSessionId", ScriptSessionId},
                {"c0-scriptName", "MaxRemoteApi"},
                {"c0-methodName", name},
                {"c0-id", "0"}
            };
            if (extraParams != null)
                foreach (var extraParam in extraParams)
                    parameters.Add(extraParam.Key, extraParam.Value);

            for (var i = 0; i < arguments.Length; i++)
            {
                parameters.Add("c0-param" + i, arguments[i]);
            }
            parameters.Add("batchId", Convert.ToString(BatchId));

            var result = RequestExecutor.Post(BaseUrl + "dwr/call/plaincall/MaxRemoteApi.login.dwr", parameters);

            if (result.Contains("message=\"Subject is not authenticated\""))
            {
                if (User == null || Password == null)
                    throw new ApplicationException("Login required");

                Relogin();

                return ExecuteApiMethod(name, extraParams, arguments);
            }

            HttpSessionId = RequestExecutor.GetSessionId(BaseUrl);

            if (result.Contains("MaxClientException"))
            {
                var message = "Command execution failed: " + name;
                var messageIdx = result.IndexOf("message=\"");
                if (messageIdx > 0)
                {
                    messageIdx += 9;
                    message += " -> " + JsonConvert.DeserializeObject(result.Substring(messageIdx, result.IndexOf("\"", messageIdx) - messageIdx));
                }
                throw new MaxCommandFailedException(message);
            }

            if (!result.Contains("dwr.engine._remoteHandleCallback('" + BatchId + "'"))
                throw new MaxCommandFailedException("Command execution failed: " + name + ". Unexpected result: " + result);

            return result;
        }

        private static readonly Regex OriginalSCriptSessionIdRegex = new Regex("dwr.engine._origScriptSessionId = \"(?<sessionId>[A-Z0-9]+?)\";");

        void initialize()
        {
            RequestExecutor = new RequestExecutor();
            BatchId = 0;

            var engineScriptUrl = BaseUrl + "dwr/engine.js";
            var script = RequestExecutor.Get(engineScriptUrl);

            HttpSessionId = RequestExecutor.GetSessionId(BaseUrl);

            var match = OriginalSCriptSessionIdRegex.Match(script);
            if (match.Success)
            {
                var origScriptSessionId = match.Groups["sessionId"].Value;
                ScriptSessionId = origScriptSessionId + new Random().Next();
            }
            else
            {
                throw new MaxProtocolException("Missing 'dwr.engine._origScriptSessionId' in " + engineScriptUrl, script);
            }
        }
    }
}