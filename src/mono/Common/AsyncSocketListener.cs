using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Chartoscope.Core
{
	// State object for reading client data asynchronously
	public enum ListenerStateOption
	{
		Unknown,
		Ready,
		Busy		
	}
	
	public class StateObject {
	    // Client  socket.
	    public Socket workSocket = null;
	    // Size of receive buffer.
	    public const int BufferSize = 1024;
	    // Receive buffer.
	    public byte[] buffer = new byte[BufferSize];
	// Received data string.
	    public StringBuilder sb = new StringBuilder();  
	}
	
	public class AsyncSocketListener {
	    // Thread signal.
	    public ManualResetEvent allDone = new ManualResetEvent(false);
		
		public ManualResetEvent receiveDone = new ManualResetEvent(false);
		
		private ListenerStateOption _state= ListenerStateOption.Unknown;
		
		public ListenerStateOption State { get { return _state; } }
		
	    public AsyncSocketListener() {
	    }
	
	    public void StartListening() {
	        // Data buffer for incoming data.
	        byte[] bytes = new Byte[1024];
	
	        // Establish the local endpoint for the socket.
	        // The DNS name of the computer
	        // running the listener is "host.contoso.com".
	        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
	        IPAddress ipAddress = ipHostInfo.AddressList[0];
	        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
	
	        // Create a TCP/IP socket.
	        Socket listener = new Socket(AddressFamily.InterNetwork,
	            SocketType.Stream, ProtocolType.Tcp );
	
	        // Bind the socket to the local endpoint and listen for incoming connections.
	        try {
	            listener.Bind(localEndPoint);				;
	            listener.Listen(100);				
								
	            while (true) {	  
					allDone.Reset();
						
					// Start an asynchronous socket to listen for connections.
	                Console.WriteLine("Waiting for a connection...");
	                listener.BeginAccept( 
	                    new AsyncCallback(AcceptCallback),
	                    listener );
					
	                // Wait until a connection is made before continuing.
					_state= ListenerStateOption.Ready;
	                allDone.WaitOne();
					_state= ListenerStateOption.Busy;
	            }
	
	        } catch (Exception e) {
	            Console.WriteLine(e.ToString());
	        }
	
	        Console.WriteLine("\nPress ENTER to continue...");
	        Console.Read();
	        
	    }
	
	    public void AcceptCallback(IAsyncResult ar) {
	        // Get the socket that handles the client request.
	        Socket listener = (Socket) ar.AsyncState;
	        Socket handler = listener.EndAccept(ar);
			
			// Signal the main thread to continue.
	        allDone.Set();
	
	        

				// Create the state object.
		        StateObject state = new StateObject();
		        state.workSocket = handler;

				// Start an asynchronous socket to listen for connections.
	            Console.WriteLine("Waiting for a data...");
	        	handler.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,
	            	new AsyncCallback(ReadCallback), state);

	    }
	
	    public void ReadCallback(IAsyncResult ar) {
	        String content = String.Empty;
	        
	        // Retrieve the state object and the handler socket
	        // from the asynchronous state object.
	        StateObject state = (StateObject) ar.AsyncState;
	        Socket handler = state.workSocket;
	
	        // Read data from the client socket. 
	        int bytesRead = handler.EndReceive(ar);
						
	        if (bytesRead > 0) {
	            // There  might be more data, so store the data received so far.
	            state.sb.Append(Encoding.ASCII.GetString(
	                state.buffer,0,bytesRead));
	
	            // Check for end-of-file tag. If it is not there, read 
	            // more data.
	            content = state.sb.ToString();
	            if (content.IndexOf("<EOF>") > -1) {
	                // All the data has been read from the 
	                // client. Display it on the console.
					
	                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
	                    content.Length, content );
	                // Echo the data back to the client.
	                Send(handler, content);							
	            } else {
	                // Not all data received. Get more.
	                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
	                new AsyncCallback(ReadCallback), state);
	            }
	        }
	    }
	    
	    private void Send(Socket handler, String data) {
	        // Convert the string data to byte data using ASCII encoding.
	        byte[] byteData = Encoding.ASCII.GetBytes(data);
	
	        // Begin sending the data to the remote device.
	        handler.BeginSend(byteData, 0, byteData.Length, 0,
	            new AsyncCallback(SendCallback), handler);
	    }
	
	    private void SendCallback(IAsyncResult ar) {
	        try {
	            // Retrieve the socket from the state object.
	            Socket handler = (Socket) ar.AsyncState;
	
	            // Complete sending the data to the remote device.
	            int bytesSent = handler.EndSend(ar);
	            Console.WriteLine("Sent {0} bytes to client.", bytesSent);
								
	            handler.Shutdown(SocketShutdown.Both);
	            handler.Close();
				//receiveDone.Set();
	
	        } catch (Exception e) {
	            Console.WriteLine(e.ToString());
	        }
	    }
	
	
//	    public static int Main(String[] args) {
//	        StartListening();
//	        return 0;
//	    }
	}
}