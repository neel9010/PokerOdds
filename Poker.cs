using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace PokerWinning
{
    public class Card
    {
        public int PLAYER_ID;
        public string SUIT = "";
        public string NUMBER = "";
        public string CARD = "";
        public string TYPE = "";
    }

    public class Player
    {
        public int ID;
        public string NAME = "";
        public int GAME_WON = 0;
        public bool R_FLUSH = false;
        public bool S_FLUSH = false;
        public bool FOUR = false;
        public bool FLUSH = false;
        public bool STRAIGHT = false;
        public bool THREE = false;
        public bool TWO_PAIR = false;
        public bool PAIR = false;
        public bool HIGH_CARD = false;

        public List<Card> Cards = new List<Card>();
        public List<Card> PossibleCards = new List<Card>();
        public List<Card> BestCards = new List<Card>();
    }

    public class Poker
    {
        private static List<Card> New_Deck = new List<Card>();
        private static List<Card> Game_Deck = new List<Card>();
        private static List<Card> Table_Cards = new List<Card>();
        private static List<Player> Players_List = null;

        private static void Main(string[] args)
        {
            Stopwatch sw;

            Deck.Initialize_New_Deck(New_Deck);
            Deck.Set_Game_Deck();

            Console.WriteLine("---------------- Welcome to game of Texas Hold'em Poker ----------------");
            Console.WriteLine();

            Console.Write("Enter Number of players (Max 10): ");
            string players = Console.ReadLine();
            Console.WriteLine();

            StartGame.Create_Players(int.Parse(players));

            Console.Write("Do you want to let computer pick cards? (Y/N) : ");
            string input = Console.ReadLine();
            Console.WriteLine();

            StartGame.Deal_Table_Cards(input.ToUpper());
            StartGame.Deal_Players_Card(input.ToUpper());

            sw = Stopwatch.StartNew();
            StartGame.Begin_Game();
            sw.Stop();
            Console.WriteLine((sw.ElapsedMilliseconds / 1000m).ToString());

            Console.Read();
        }

        private class Deck
        {
            private static string suit = "C";
            private static int suit_num = 1;
            private static int card_num = 0;

            public static void Initialize_New_Deck(List<Card> New_Deck)
            {
                for (int i = 1; i < 14; i++)
                {
                    Card card = new Card();
                    card.TYPE = "NewDeck";
                    card.SUIT = suit;
                    card.NUMBER = set_card_value(i);
                    card.CARD = card.NUMBER + card.SUIT;
                    New_Deck.Add(card);

                    if (i == 13)
                    {
                        if (suit_num == 4)
                            break;

                        i = 0;
                        suit_num++;
                        suit = set_suit(suit_num);
                    }

                    card_num++;
                }
            }

            public static void Set_Game_Deck()
            {
                Game_Deck = New_Deck;
            }

            public static string set_card_value(int i)
            {
                switch (i)
                {
                    case 0: return "10";
                    case 1: return "A";
                    case 11: return "J";
                    case 12: return "Q";
                    case 13: return "K";
                    default: return i.ToString();
                }
            }

            private static string set_suit(int suit_num)
            {
                return suit_num == 2 ? "S" : suit_num == 3 ? "D" : "H";
            }
        }

        protected internal class StartGame
        {
            private string CARD_1 { get; set; }
            private string CARD_2 { get; set; }
            private string CARD_3 { get; set; }

            public static void Create_Players(int num)
            {
                for (int i = 0; i < num; i++)
                {
                    Player player = new Player();
                    player.ID = i;
                    player.NAME = Set_Players_Name(i + 1).ToUpper();

                    if (Players_List == null)
                    {
                        Players_List = new List<Player>();
                    }

                    Players_List.Add(player);
                }

                Console.WriteLine("Players For this game");
                foreach (var player in Players_List)
                {
                    Console.WriteLine(player.NAME);
                }

                Console.WriteLine();
                Console.WriteLine("TOTAL CARDS IN DECK :" + New_Deck.Count());
                Console.WriteLine("-------------------------------");
            }

            public static void Deal_Table_Cards(string autopick = "Y")
            {
                StartGame Game = new StartGame();

                for (int card_num = 1; card_num < 4; card_num++)
                {
                    int random_card;

                    Card Card = new Card();

                    string input = null;

                    if (autopick.Trim().StartsWith("Y"))
                    {
                        Random r = new Random();
                        random_card = r.Next(0, New_Deck.Count());
                        Card = New_Deck[random_card];
                    }
                    else
                    {
                        Console.Write("Enter Suit (C\\H\\D\\S) " + " : ");
                        input = Console.ReadLine();

                        Card.SUIT = input.ToUpper();

                        Console.Write("Enter Table Card " + card_num + " : ");
                        input = Console.ReadLine();

                        Card.NUMBER = Deck.set_card_value(int.Parse(input)).ToString(); ;
                        Console.WriteLine();
                    }

                    Card.CARD = Card.NUMBER + Card.SUIT;
                    Card.TYPE = "- Visible On Table";
                    Table_Cards.Add(Card);

                    Set_Table_Cards(card_num, Game, Card.CARD);

                    Card used_card = Game_Deck.Where(x => x.CARD == Card.CARD).FirstOrDefault();
                    Game_Deck.Remove(used_card);

                    Game_Deck.Shuffle_Deck();
                }

                Console.WriteLine("CARDS ON TABLE");
                Console.WriteLine("CARD 1 : " + Game.CARD_1);
                Console.WriteLine("CARD 2 : " + Game.CARD_2);
                Console.WriteLine("CARD 3 : " + Game.CARD_3);
                Console.WriteLine("-------------------------------");
                Console.WriteLine();
                Console.WriteLine("REMAINING CARDS AFTER TABLE DEAL :" + Game_Deck.Count());
                Console.WriteLine("-------------------------------");
            }

            public static void Deal_Players_Card(string autopick = "")
            {
                foreach (var player in Players_List)
                {
                    int random_card;

                    string input = "";
                    string suit = "";
                    if (!autopick.Trim().StartsWith("Y"))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Enter Cards For " + player.NAME);

                        Console.Write("Enter Suit (C\\H\\D\\S) " + " : ");
                        suit = Console.ReadLine();
                    }

                    for (int i = 1; i < 3; i++)
                    {
                        Card player_card = new Card();

                        if (autopick.Trim().StartsWith("Y"))
                        {
                            Random r = new Random();
                            random_card = r.Next(0, Game_Deck.Count());
                            player_card = Game_Deck[random_card];
                        }
                        else
                        {
                            player_card.SUIT = suit.ToUpper();

                            Console.Write("Card " + i + " : ");
                            input = Console.ReadLine();
                            player_card.NUMBER = Deck.set_card_value(int.Parse(input)).ToString();

                            player_card.CARD = player_card.NUMBER + player_card.SUIT;
                        }

                        player_card.TYPE = "- In Player's Hand";
                        player_card.PLAYER_ID = player.ID;
                        player.Cards.Add(player_card);

                        Card used_card = Game_Deck.Where(x => x.CARD == player_card.CARD).FirstOrDefault();
                        Game_Deck.Remove(used_card);

                        Card possible_card;
                        possible_card = new Card();
                        possible_card.CARD = player_card.CARD;
                        possible_card.SUIT = player_card.SUIT;
                        possible_card.NUMBER = player_card.NUMBER;
                        possible_card.TYPE = player_card.TYPE;
                        possible_card.PLAYER_ID = player.ID;
                        player.PossibleCards.Add(possible_card);

                        Game_Deck.Shuffle_Deck();
                    }
                }

                foreach (var player in Players_List)
                {
                    var player_cards_list = player.Cards.Where(x => x.PLAYER_ID == player.ID).ToList();
                    var card_count = 1;

                    Console.WriteLine("Dealt Cards for Player : " + player.NAME);

                    foreach (var card in player_cards_list)
                    {
                        Console.WriteLine("CARD" + card_count + ":" + card.CARD);
                        card_count++;
                    }

                    Console.WriteLine("-------------------------------");
                }

                Console.WriteLine();
                Console.WriteLine("REMAINING CARDS AFTER PLAYER'S DEAL :" + Game_Deck.Count());
                Console.WriteLine("-------------------------------");

                Console.Write("Press Any Key to Continue...");
                Console.ReadLine();
            }

            public static void Begin_Game()
            {
                List<Card> possible_seven = new List<Card>();
                Game_Deck.Shuffle_Deck();
                var temp_card_list = Game_Deck.ToList();

                var combination = 1;
                int count = Game_Deck.Count();

                do
                {
                    int deal_card;
                    Card hidden_card = new Card();

                    Random r = new Random();
                    deal_card = r.Next(0, Game_Deck.Count());
                    hidden_card = Game_Deck[deal_card];
                    hidden_card.TYPE = "- First Possible Hidden Card on Table";
                    possible_seven.Add(hidden_card);

                    var temp_card = temp_card_list[deal_card];
                    temp_card_list.Remove(temp_card);

                    Card used_card = Game_Deck.Where(x => x.CARD == temp_card.CARD).FirstOrDefault();
                    Game_Deck.Remove(used_card);

                    count = temp_card_list.Count();

                    foreach (var Card in Game_Deck.ToList())
                    {
                        Console.WriteLine("------------- COMBINATION - " + combination);
                        Console.WriteLine("-------------------------------");

                        hidden_card = new Card();
                        hidden_card.CARD = Card.CARD;
                        hidden_card.TYPE = "- Second Possible Hidden Card on Table";
                        possible_seven.Add(hidden_card);

                        foreach (var player in Players_List)
                        {
                            foreach (var tc_item in Table_Cards)
                            {
                                Card possible_card = new Card();
                                possible_card = tc_item;
                                possible_card.TYPE = tc_item.TYPE;
                                possible_card.PLAYER_ID = player.ID;
                                player.PossibleCards.Add(possible_card);
                            }

                            foreach (var poss_seven in possible_seven)
                            {
                                Card possible_card = new Card();
                                possible_card = poss_seven;
                                possible_card.TYPE = poss_seven.TYPE;
                                possible_card.PLAYER_ID = player.ID;
                                player.PossibleCards.Add(possible_card);
                            }

                            Console.WriteLine("POSSIBLE 7 CARDS FOR " + player.NAME);
                            Console.WriteLine();

                            foreach (var card in player.PossibleCards)
                            {
                                Console.WriteLine(card.CARD + " " + card.TYPE);
                            }

                            Check_Best_Hand(player);

                            var deletion_list = player.PossibleCards.Skip(2).ToList();
                            foreach (var pos_card in deletion_list)
                            {
                                player.PossibleCards.Remove(pos_card);
                            }

                            Console.WriteLine("-------------------------------");
                        }

                        possible_seven.Remove(hidden_card);
                        Game_Deck.Shuffle_Deck();

                        Console.WriteLine("CARDS LEFT IN DECK - " + (temp_card_list.Count - 1));
                        Console.WriteLine("-------------------------------");
                        combination++;
                    }

                    possible_seven.Clear();
                    Game_Deck = temp_card_list;
                    Game_Deck.Shuffle_Deck();
                    count--;
                } while (count > 0);
            }

            public static void Set_Table_Cards(int card_num, StartGame game, string card)
            {
                if (card_num == 1) { game.CARD_1 = card; } else if (card_num == 2) { game.CARD_2 = card; } else { game.CARD_3 = card; }
            }

            public static string Set_Players_Name(int player_num)
            {
                switch (player_num)
                {
                    case 1: return "Neal";
                    case 2: return "Jasmin";
                    case 3: return "Gus";
                    case 4: return "Patric";
                    case 5: return "Mayme";
                    case 6: return "Kyong";
                    case 7: return "Lan";
                    case 8: return "Aldo";
                    case 9: return "Lan";
                    case 10: return "Anisa";
                    default: return "Player_" + player_num.ToString();
                }
            }

            //TODO Not the best way to do it and incomplete
            public static void Check_Best_Hand(Player player)
            {
                var players_cards = player.PossibleCards.OrderByDescending(x => x.CARD.ToString()).ToList();

                var royal_list = players_cards.Where(x => x.SUIT == "D").ToList();
                player.R_FLUSH = royal_list.Count < 5 ? false : royal_list.RoyalFlush(player);

                //Checks Royal Flush
                if (player.R_FLUSH)
                {
                    player.GAME_WON++;
                    return;
                }

                //Checks Straight Flush
                player.S_FLUSH = players_cards.StraightSuit(player);
                if (player.S_FLUSH)
                    return;

                //Checks Four of Kind
                player.FOUR = players_cards.Four_of_kind(player);
                if (player.FOUR)
                    return;

                //Checks Flush
                player.FLUSH = players_cards.Flush(player);
                if (player.FLUSH)
                    return;

                //TODO Straight

                //Checks Three of Kind
                player.THREE = players_cards.Three_of_kind(player);

                //TODO Two Pairs

                //TODO Pairs

                //Checks High Card  if non of the above
                player.HIGH_CARD = players_cards.High_card(player);
            }
        }
    }

    public static class Extensions
    {
        public static void Shuffle_Deck<T>(this IList<T> list)
        {
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static bool RoyalFlush(this List<Card> PossibleCards, Player player)
        {
            int card_count = 0;
            foreach (var card in PossibleCards)
            {
                var match = Regex.Matches(card.NUMBER.ToUpper(), @"([A]|[K]|[Q]|[J]|[10])");

                if (match.Count > 0)
                {
                    Add_Best_Cards(card, player);
                    card_count++;
                }
            }

            if (card_count != 5)
            {
                player.BestCards.Clear();
            }

            return card_count == 5 ? true : false;
        }

        public static bool StraightSuit(this List<Card> PossibleCards, Player player)
        {
            int dimond_count = PossibleCards.Where(x => x.SUIT == "D").Count();
            int heart_count = PossibleCards.Where(x => x.SUIT == "H").Count();
            int club_count = PossibleCards.Where(x => x.SUIT == "C").Count();
            int spade_count = PossibleCards.Where(x => x.SUIT == "S").Count();

            string suit = dimond_count > 4 ? "D" : heart_count > 4 ? "H" : club_count > 4 ? "C" : spade_count > 4 ? "S" : "";

            var straight_list = PossibleCards.Where(x => x.SUIT == suit).Take(5).ToList();

            foreach (var card in straight_list)
            {
                Add_Best_Cards(card, player);
            }

            return straight_list.Count > 4 ? true : false;
        }

        public static bool Four_of_kind(this List<Card> PossibleCards, Player player)
        {
            var check_list = PossibleCards.ToList();

            foreach (var check_card in check_list)
            {
                var temp_list = PossibleCards.Where(x => x.NUMBER == check_card.NUMBER).ToList();

                if (temp_list.Count() >= 4)
                {
                    foreach (var card in temp_list.Take(5))
                    {
                        Add_Best_Cards(card, player);
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool Three_of_kind(this List<Card> PossibleCards, Player player)
        {
            var check_list = PossibleCards.ToList();

            foreach (var check_card in check_list)
            {
                var temp_list = PossibleCards.Where(x => x.NUMBER == check_card.NUMBER).ToList();

                if (temp_list.Count() >= 3)
                {
                    foreach (var card in temp_list.Take(5))
                    {
                        Add_Best_Cards(card, player);
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool Flush(this List<Card> PossibleCards, Player player)
        {
            var check_list = PossibleCards.ToList();

            foreach (var check_card in check_list)
            {
                var temp_list = PossibleCards.Where(x => x.SUIT == check_card.SUIT).ToList();

                if (temp_list.Count() >= 4)
                {
                    foreach (var card in temp_list.Take(5))
                    {
                        Add_Best_Cards(card, player);
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool High_card(this List<Card> PossibleCards, Player player)
        {
            foreach (var card in PossibleCards.Take(5))
            {
                Add_Best_Cards(card, player);
            }

            return true;
        }

        public static void Add_Best_Cards(Card card, Player player)
        {
            Card best_card = new Card();
            best_card = card;
            player.BestCards.Add(best_card);
        }
    }
}
