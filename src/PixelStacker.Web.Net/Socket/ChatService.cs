using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace PixelStacker.Web.Net.Socket
{
    public enum OpCodeType
    {
        None,
        Chat,
        Click
    }

    public class OpCode
    {
        public OpCodeType OpType { get; set; }
        public object? Data { get; set; }
    }

    public static class WebSocketExtensions
    {
        /// <summary>
        /// Binds to /ws
        // new WebSocket("wss://localhost:53739/socket")
        // new WebSocket("ws://localhost:5005/socket")
        // new WebSocket(`wss://taylorlove.info/projects/pixelstacker/ws`)
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebSocketChatService(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (!context.WebSockets.IsWebSocketRequest)
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("Expected a WebSocket request");
                    }

                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    var chatService = context.RequestServices.GetRequiredService<ChatService>();
                    await chatService.HandleWebSocketConnection(webSocket);
                }

                await next();
            });
        }
    }

    public class ChatService
    {
        private readonly object _lock = new();
        private readonly List<WebSocket> _sockets = new();

        public async Task HandleWebSocketConnection(WebSocket socket)
        {
            AddSocket(socket);

            var buffer = new byte[1024 * 2];

            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await socket.CloseAsync(result.CloseStatus!.Value, result.CloseStatusDescription, CancellationToken.None);
                        break;
                    }

                    var messageJson = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    
                    try
                    {
                        OpCode op = JsonSerializer.Deserialize<OpCode>(messageJson);
                        if (op != null)
                        {
                            await HandleOpCode(socket, op);
                        }
                    }
                    catch (JsonException)
                    {
                        // Optionally log and skip invalid input
                        return;
                    }
                }
            }
            catch
            {
                // Log if needed
            }
            finally
            {
                RemoveSocket(socket);
                try { await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None); } catch { }
            }
        }

        private async Task HandleOpCode(WebSocket socket, OpCode op)
        {
            switch (op.OpType)
            {
                case OpCodeType.Chat:
                    // Broadcast to everyone else
                    string msg = op.Data as string;
                    byte[] bytes = Encoding.UTF8.GetBytes(msg);
                    await SendAll(socket, bytes);
                    break;

                case OpCodeType.Click:
                    // Handle Click logic here
                    break;

                default:
                    break;
            }
        }


        private async Task SendAll(WebSocket socket, OpCode op)
        {
            string json = JsonSerializer.Serialize(op, JsonSerializerOptions.Default); 
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            SendAll(socket, bytes); 
        }

        private async Task SendAll(WebSocket socket, byte[] message)
        {
            List<WebSocket> sockets;
            lock (_lock)
            {
                sockets = _sockets.ToList();
            }

            var sendTasks = _sockets.Select(async s =>
            {
                if (s != socket && s.State == WebSocketState.Open)
                {
                    try
                    {
                        await s.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch
                    {
                        RemoveSocket(s);
                        try { await s.CloseAsync(WebSocketCloseStatus.InternalServerError, "Send failed", CancellationToken.None); } catch { }
                    }
                }
            });

            await Task.WhenAll(sendTasks);
        }

        private void AddSocket(WebSocket socket)
        {
            lock (_lock)
            {
                _sockets.Add(socket);
            }
        }

        private void RemoveSocket(WebSocket socket)
        {
            lock (_lock)
            {
                _sockets.Remove(socket);
            }
        }
    }

    //public class ChatService_OLD
    //{
    //    private readonly List<WebSocket> _sockets = new();

    //    public async Task HandleWebSocketConnection(WebSocket socket)
    //    {
    //        _sockets.Add(socket);
    //        var buffer = new byte[1024 * 2];
    //        while (socket.State == WebSocketState.Open)
    //        {
    //            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), default);
    //            if (result.MessageType == WebSocketMessageType.Close)
    //            {
    //                await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, default);
    //                break;
    //            }

    //            foreach (var s in _sockets)
    //            {
    //                await s.SendAsync(buffer[..result.Count], WebSocketMessageType.Text, true, default);
    //            }
    //        }
    //        _sockets.Remove(socket);
    //    }
    //}
}
