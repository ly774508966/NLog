using System;
using NLog;
using System.Net;

namespace Example {
    public class Program {
        public static void Main(string[] args) {
            LoggerFactory.globalLogLevel = LogLevel.On;
            consoleLogTest();
            //fileWriterTest();
            //clientSocketTest();
            //serverSocketTest();

            Log.Trace("trace");
            Log.Debug("debug");
            Log.Info("info");
            Log.Warn("warn");
            Log.Error("error");
            Log.Fatal("fatal");

            Console.Read();
        }

        static void consoleLogTest() {
            var formatter = new DefaultLogMessageFormatter();
            var colorFormatter = new ColorCodeFormatter();
            LoggerFactory.appenders += (logger, logLevel, message) => {
                var logMessage = formatter.FormatMessage(logger, logLevel, message);
                var coloredLogMessage = colorFormatter.FormatMessage(logLevel, logMessage);
                Console.WriteLine(coloredLogMessage);
            };
        }

        static void fileWriterTest() {
            var fileWriter = new FileWriter("Log.txt");
            fileWriter.ClearFile();
            var formatter = new DefaultLogMessageFormatter();
            var colorFormatter = new ColorCodeFormatter();
            LoggerFactory.appenders += (logger, logLevel, message) => {
                var logMessage = formatter.FormatMessage(logger, logLevel, message);
                var coloredLogMessage = colorFormatter.FormatMessage(logLevel, logMessage);
                fileWriter.WriteLine(coloredLogMessage);
            };
        }

        static void clientSocketTest() {
            // Connect
            // $ nc -lp 1234
            var formatter = new DefaultLogMessageFormatter();
            var colorFormatter = new ColorCodeFormatter();
            var socket = new SocketAppender();
            LoggerFactory.appenders += ((logger, logLevel, message) => {
                var logMessage = formatter.FormatMessage(logger, logLevel, message);
                var coloredLogMessage = colorFormatter.FormatMessage(logLevel, logMessage);
                socket.Send(logLevel, coloredLogMessage);
            });

            socket.Connect(IPAddress.Loopback, 1234);
        }

        static void serverSocketTest() {
            // Connect
            // $ telnet 127.0.0.1 1234
            var formatter = new DefaultLogMessageFormatter();
            var colorFormatter = new ColorCodeFormatter();
            var socket = new SocketAppender();
            LoggerFactory.appenders += ((logger, logLevel, message) => {
                var logMessage = formatter.FormatMessage(logger, logLevel, message);
                var coloredLogMessage = colorFormatter.FormatMessage(logLevel, logMessage);
                socket.Send(logLevel, coloredLogMessage);
            });

            socket.Listen(1234);
        }
    }
}
