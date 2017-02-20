using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CustomNetworking;
using BB;
using System.IO;

namespace BoggleServer {

    /// <summary>
    /// The BoggleServer Server. Contains the base Player class. Provides a connection among the clients that
    /// want to play the game, handles game states, and controls all console outputs.
    /// </summary>
    public class BoggleServer {
        /// <summary>
        /// Listens for incoming connections
        /// </summary>
        private TcpListener server;
        /// <summary>
        /// All the waitingPlayers that are currently connected
        /// </summary>
        private Queue<Player> waitingPlayers;
            // TODO: this queue only holds 2 players max.  Refactor to use a simpler data structure.
        /// <summary>
        ///Contains the sessions currently on the server.
        /// </summary>
        private List<GameSession> sessions;
        /// <summary>
        ///  board to use for all game sessions if the letters are passed in
        /// </summary>
        private BoggleBoard universalBoard;
        /// <summary>
        /// The dictionary that holds all possible words
        /// </summary>
        private readonly HashSet<String> dictionary;
        /// <summary>
        /// The number of seconds a game will last.
        /// </summary>
        private int secondsPerGame;

        public BoggleServer(String letters, int seconds, string filepath) : this(seconds, filepath){
            universalBoard = new BoggleBoard(letters);
        }

        /// <summary>
        /// Main constructor of a BoggleServer server object. Provides connection among clients.
        /// </summary>
        public BoggleServer(int seconds, string filepath) {
            //Intialize the server, and begin accepting socket connections.
            //System.Diagnostics.Debug.WriteLine("Starting the Boggle Server");
            server = new TcpListener(IPAddress.Any, 2000);
            secondsPerGame = seconds;
            sessions = new List<GameSession>();
            dictionary = new HashSet<String>();
            waitingPlayers = new Queue<Player>();

            //Begin the server, and start accepting sockets.
            server.Start();
            server.BeginAcceptSocket(ConnectionReceived, null);

            StreamReader reader = new StreamReader(filepath);

            //Fill the hashset with the dictionary for parsing.
            while (!reader.EndOfStream) {
                dictionary.Add(reader.ReadLine());
            }
            ThreadPool.QueueUserWorkItem((x) => gamesRunning());
        }

        /// <summary>
        /// Deals with connection requests
        /// </summary>
        private void ConnectionReceived(IAsyncResult ar) {
            System.Diagnostics.Debug.WriteLine("Received a Connection");
            Socket socket = server.EndAcceptSocket(ar);
            StringSocket ss = new StringSocket(socket, UTF8Encoding.Default);
            ss.BeginReceive(NameReceived, ss);
            server.BeginAcceptSocket(ConnectionReceived, null);
        }

        /// <summary>
        /// Callback that creates a new Player with the given nameMsg and Socket.
        /// </summary>
        /// <param name="e">An exception if there was an error getting the name</param>
        /// <param name="nameMsg">The message that contains the name of the player</param>
        /// <param name="playerSocket">
        /// The payload from the StringSocket, which contains the 
        /// StringSocket used to communicate with the player
        /// </param>
        private void NameReceived(String nameMsg, Exception e, object playerSocket) {
            if (nameMsg == null) { return; }

            System.Diagnostics.Debug.WriteLine("Receiving a player's name");
            StringSocket ss = (StringSocket)playerSocket;

            // nameMsg should be in the format: PLAY (Name of Player)
            if(nameMsg.StartsWith("PLAY ")) {
                String playerName = nameMsg.Substring("PLAY ".Length);
                lock (waitingPlayers) {
                    Player p = new Player(ss, playerName);
                    waitingPlayers.Enqueue(p);

                    if (waitingPlayers.Count % 2 == 0) {
                        System.Diagnostics.Debug.WriteLine("Now pairing 2 players");
                        Player p1 = waitingPlayers.Dequeue();
                        if (p1.Socket.Connected) {
                            Player p2 = waitingPlayers.Dequeue();
                            ThreadPool.QueueUserWorkItem((x) => init(p1, p2));
                            
                        }
                    }
                    // Question: how would you notify other clients of the new person joining?
                }
            }
            else {
                // player hasn't been created yet, so we can't call the player's ignoring method
                ss.BeginSend("IGNORING " + nameMsg, sendCallbackCheck, ss);
            }
        }

        private void sendCallbackCheck(Exception e, object o) {
            
        }

        private void init(Player p1, Player p2) {
            if (universalBoard == null) {
                sessions.Add(new GameSession(p1, p2, secondsPerGame,dictionary));
            }
            else {
                sessions.Add(new GameSession(p1, p2, universalBoard, secondsPerGame,dictionary));
            }
        }

        private void gamesRunning()
        {
            while (true)
            {
                Console.WriteLine("There are " + sessions.Count + " games currently running.");
                Thread.Sleep(10000);
            }
        }

        /// <summary>
        /// Starts a new BoggleServer server with the given 
        /// </summary>
        /// <param name="args">
        /// The number of seconds a game should last
        /// The pathname of a file that contains all legal words.  
        /// The file should contain one word per line.
        /// An optional string consisting of exactly 16 letters used to initialize the boggle board.
        /// </param>
        public static void Main(string[] args)
        {

            int secondsPerGame = Convert.ToInt32(args[0]);
            String pathToDictionary = args[1];

            if(args.Length > 2){
                String letters = args[2];
                new BoggleServer(letters, secondsPerGame, pathToDictionary);
            }
            else new BoggleServer(secondsPerGame, pathToDictionary);

            
            Console.Read();
        }
    }
}