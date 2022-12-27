using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.IO;
using System.Net;
using NetCoreApiRest.Utils;
using System.Diagnostics;

namespace NetCoreApiRest.Utils{
	public class LoggerD4{
		/*
		*	This class is a custom solution for trace logs and error log to help debug,
		*	identify problems and error and find solutions. It has tre options for loggin,
		*	these options can be used simultaneosly, only two or just one, the options are:
		*		Service: In this case it rely on Logz.io for login, the URL is already hardcoded.
		*		File: All the log lines are going to be writen to he specified file in the given path.
		*		Console: Show the messages in the Output console, useful only while developing.
		*	The log level can be limited to avoid having too much data to deal with, currently the levels are
		*	TRACE, DEBUG, INFO, WARNING, ERROR and FATAL, in that order from less to more, when a level
		*	is set it will include all the above it, but ignore the one bellow, this will allow to add
		*	all the trace log need it to debug and find problems and when that is solved those can be disabled
		*	without the need of edit all the code, and if a new problem arise the they can be reactivate, it's 
		*	recomended to add all the relevent log calls while developing and control the volumen here.
		*	Example: If logLevel is set to INFO, then TRACE and DEBUG won't be recorded, but INFO, WARNING,
		*	ERROR AND FATAL will.
		*/

		private String className;
		private String level = "";
		private String sessionId = ""; //This string it's used to identify a log session so a trace of a single session can be done more efficiently
		private String logToConsole = ConfigurationSite._cofiguration["LogConfig:logToConsole"]; //Activate logs for the Output console
		private String logToFile = ConfigurationSite._cofiguration["LogConfig:logToFile"]; //Write the logs in a file
		private String logToService = ConfigurationSite._cofiguration["LogConfig:logToService"]; //Send logs to the service (logz.io)
		private String token = ConfigurationSite._cofiguration["LogConfig:loggerToken"]; //Log service token
		private String logzType = ConfigurationSite._cofiguration["LogConfig:logzType"]; //To identify the source of the log in logz.io, usefull when several projects send to the same account
		private String logLevel = ConfigurationSite._cofiguration["LogConfig:logLevel"]; //The minimun log level, values: [trace, debug, info, warning, error, fatal]
		private String logPath = ConfigurationSite._cofiguration["LogConfig:logPath"];   //PathWhere to write the files, write permission need it [C:/logs/]
		private String logFile = ConfigurationSite._cofiguration["LogConfig:logFile"];   //Filename to write the logs

		public LoggerD4(String className)
		{
			//To add some context to the log, where it came from, will be prepend to the log
			this.className = className;
			//TODO: populate the sessionId based on a parameter in the consturctor so this will work globally, currently works only for each instance
			Random rnd = new Random();
			this.sessionId = rnd.Next(1, 500).ToString(); //generating a random number for simplicity, this should be a param
		}
		//These are the methods for each log level 
		public void fatal(string message)
		{
			level = "FATAL";
			prepareLog("[FATAL]: " + message, 6);
		}
		public void error(string message)
		{
			level = "ERROR";
			prepareLog("[ERROR]: " + message, 5);
		}
		public void warn(string message)
		{
			level = "WARNING";
			prepareLog("[WARNING]: " + message, 4);
		}
		public void info(string message)
		{
			level = "INFO";
			prepareLog("[INFO]: " + message, 3);
		}
		public void debug(string message)
		{
			level = "DEBUG";
			prepareLog("[DEBUG]: " + message, 2);
		}
		public void trace(string message)
		{
			level = "TRACE";
			prepareLog("[TRACE]: " + message, 1);
		}

		private void prepareLog(string message, int type)
		{
			//Prepare the log to be send, filter the levels, add the tags and 
			int minLevel = 0;
			switch (logLevel)
			{
				case "trace":
					minLevel = 1;
					break;
				case "debug":
					minLevel = 2;
					break;
				case "info":
					minLevel = 3;
					break;
				case "warning":
					minLevel = 4;
					break;
				case "error":
					minLevel = 5;
					break;
				case "fatal":
					minLevel = 6;
					break;
				default:
					minLevel = 0;
					break;
			}
			if (type >= minLevel)
			{
				//only log the level wanted, this way there is no need to remove o add later logs again
				if (logToService == "True") { }
				dynamic info = new ExpandoObject();
				String date = DateTime.Now.ToString("o");
				info.level = level;
				//info.message = "["+date+"] ["+className+"]" + message;
				info.message = "[" + className + "]" + message; //no date
				info.type = logzType;
				info.sessionId = sessionId;
				logDataService(info);
			}
			if (logToFile == "True")
			{
				String date = DateTime.Now.ToString("HH:mm:ss.ffff");
				logDataFile("[" + date + "] [" + className + "]" + message);
			}
			if (logToConsole == "True")
			{
				logConsole("[" + className + "]" + message);
			}
		}


		private void logDataFile(String data)
		{
			//process the data and write in the file
			try
			{
				String filePath = logPath + DateTime.Today.ToString("yyyy-MM-dd") + "." + logFile;
				File.AppendAllText(filePath, data + Environment.NewLine);
				/*using (StreamWriter stream = new FileInfo(filePath).AppendText()){
					 stream.WriteLine(data);
				}*/
			}
			catch (Exception e)
			{
				Debug.WriteLine("********************");
				Debug.WriteLine(e.Message);
			}
		}

		private void logConsole(String data)
		{
			//Print the message to the Output
			Debug.WriteLine(data);
		}

		private void logDataService(dynamic data)
		{
			
			//Send the log to the service
			//String url = "https://us.webhook.logs.insight.rapid7.com/v1/noformat/"+token;
			//This is the URL to send the log
			String url = "https://listener.logz.io:8071/?token=" + token + "&type=" + logzType;
			//Debug.WriteLine("Logging");
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			//set the Security protocol
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
				| SecurityProtocolType.Tls11
				| SecurityProtocolType.Tls12;
				//| SecurityProtocolType.Ssl3;

			request.Method = "POST";
			//request.ContentType = "application/json";
			//request.ContentLength = data.Length;
			using (Stream webStream = request.GetRequestStream())
			using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
			{
				requestWriter.Write(JsonConvert.SerializeObject(data));
			}
			try
			{
				WebResponse webResponse = request.GetResponse();
				using (Stream webStream = webResponse.GetResponseStream())
				{
					if (webStream != null)
					{
						using (StreamReader responseReader = new StreamReader(webStream))
						{
							string response = responseReader.ReadToEnd();
							//Debug.WriteLine(response);
						}
					}
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("-----------------");
				Debug.WriteLine(e.Message);
			}
		}
	}
}
