using CustomNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleServer
{
    /// <summary>
    /// The base Player class. Provides a client with a String Socket to connect to the server, as well
    /// as a user nameMsg, both of which are properties.
    /// </summary>
    public class Player
    {

        /// <summary>
        /// The StringSocket used to communicate with this player.
        /// </summary>
        public StringSocket Socket { get; private set; }
        /// <summary>
        /// The nameMsg of this player.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// The player's current score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// All the words that have been played by this player. 
        /// HashSet is optimal, as we want constant lookup time.
        /// </summary>
        public HashSet<String> ValidWords { get; private set; }

        /// <summary>
        /// All the words that have been played by this player. 
        /// HashSet is optimal, as we want constant lookup time.
        /// </summary>
        public HashSet<String> InvalidWords { get; private set; }

        /// <summary>
        /// Creates a new player with the given StringSocket and nameMsg
        /// </summary>
        /// <param nameMsg="socket">the StringSocket used to communicate with this player</param>
        /// <param nameMsg="nameMsg">The nameMsg of this player</param>
        public Player(StringSocket socket, String name) {
            Socket = socket;
            Name = name;
            ValidWords = new HashSet<String>();
            InvalidWords = new HashSet<String>();
        }

        /// <summary>
        /// Handles ignored/invalid commands.
        /// </summary>
        /// <param name="message"></param>
        public void Ignoring(string message) {
            System.Diagnostics.Debug.WriteLine(Name + " is ignoring command: " + message);
            Socket.BeginSend("IGNORING " + message, (e, o) => { }, this);
        }

    } //End Player class.
}
