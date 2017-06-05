//UNFINISHED CODE - WIP

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PokerWinning
{
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

        public static List<Player> players { get; set; }

        public List<Card> Cards = new List<Card>();
        public List<PossibleCards> PossibleCards = new List<PossibleCards>();
        public List<BestCards> BestCards = new List<BestCards>();
    }

    public class Card
    {
        public int player_id;
        public string card = "";
    }

    public class PossibleCards : Card { }

    public class BestCards : Card { }

    public class TableCards : Card { }

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

        public static bool RoyalFlush(this List<PossibleCards> PossibleCards, Player player)
        {
            foreach (var item in PossibleCards)
            {
                var card = item.card.ToString().Substring(0, 1).ToUpper();
                var match = Regex.Matches(card, @"([A]|[K]|[Q]|[J]|[0])");

                if (match.Count > 0)
                {
                    BestCards best_card = new BestCards();
                    best_card.player_id = player.ID;
                    best_card.card = item.card;
                    player.BestCards.Add(best_card);
                }
                else
                {
                    player.BestCards.Clear();
                    return false;
                }
            }

            return true;
        }
    }

    public class Poker
    {
        private static string[] New_Deck = new string[52];
        private static List<TableCards> Table_Cards = new List<TableCards>();
        private static List<Player> Players_List = null;
        private static List<string> Card_Deck = new List<string>();

        private static void Main(string[] args)
        {
            Deck.Initialize(New_Deck);

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

            StartGame.Begin_Game();

            Console.Read();
        }

        private class Deck
        {
            private static string suit = "C";
            private static int suit_num = 1;
            private static int card_num = 0;

            public static void Initialize(string[] deck)
            {
                for (int i = 1; i < 14; i++)
                {
                    var current_card = set_card(i) + suit;

                    if (card_num <= deck.Length)
                    {
                        deck[card_num] = current_card;
                    }

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

            private static string set_card(int i)
            {
                switch (i)
                {
                    case 1: return "A";
                    case 10: return "0";
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
                Console.WriteLine("TOTAL CARDS IN DECK :" + New_Deck.Length);
                Console.WriteLine("-------------------------------");
            }

            public static void Deal_Table_Cards(string autopick = "Y")
            {
                Card_Deck = New_Deck.ToList();
                StartGame Game = new StartGame();

                for (int card_num = 1; card_num < 4; card_num++)
                {
                    int deal_card;
                    string table_card = null;
                    if (autopick.Trim().StartsWith("Y"))
                    {
                        Random r = new Random();
                        deal_card = r.Next(0, Card_Deck.Count());
                        table_card = Card_Deck[deal_card];
                    }
                    else
                    {
                        Console.Write("Enter Table Card " + card_num + " : ");
                        table_card = Console.ReadLine();

                        table_card = table_card.ToUpper();
                    }

                    TableCards tablecard = new TableCards();
                    tablecard.card = table_card;
                    Table_Cards.Add(tablecard);

                    Set_Table_Cards(card_num, Game, table_card);
                    Card_Deck.Remove(table_card);
                    Card_Deck.Shuffle_Deck();
                }

                Console.WriteLine("CARDS ON TABLE");
                Console.WriteLine("CARD 1 : " + Game.CARD_1);
                Console.WriteLine("CARD 2 : " + Game.CARD_2);
                Console.WriteLine("CARD 3 : " + Game.CARD_3);
                Console.WriteLine("-------------------------------");
                Console.WriteLine();
                Console.WriteLine("REMAINING CARDS AFTER TABLE DEAL :" + Card_Deck.Count());
                Console.WriteLine("-------------------------------");
            }

            public static void Deal_Players_Card(string autopick = "")
            {
                foreach (var player in Players_List)
                {
                    int deal_card;
                    string player_card = null;

                    if (!autopick.Trim().StartsWith("Y"))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Enter Cards For " + player.NAME);
                    }

                    for (int i = 1; i < 3; i++)
                    {
                        if (autopick.Trim().StartsWith("Y"))
                        {
                            Random r = new Random();
                            deal_card = r.Next(0, Card_Deck.Count());
                            player_card = Card_Deck[deal_card];
                        }
                        else
                        {
                            Console.Write("Card " + i + " : ");
                            player_card = Console.ReadLine();
                        }

                        Set_Player_Card(player, player_card);
                        Card_Deck.Shuffle_Deck();
                    }
                }

                foreach (var player in Players_List)
                {
                    var player_cards_list = player.Cards.Where(x => x.player_id == player.ID).ToList();
                    var card_count = 1;

                    Console.WriteLine("Dealt Cards for Player : " + player.NAME);

                    foreach (var card in player_cards_list)
                    {
                        Console.WriteLine("CARD" + card_count + ":" + card.card);
                        card_count++;
                    }

                    Console.WriteLine("-------------------------------");
                }

                Console.WriteLine();
                Console.WriteLine("REMAINING CARDS AFTER PLAYER'S DEAL :" + Card_Deck.Count());
                Console.WriteLine("-------------------------------");
                Console.ReadLine();
            }

            public static void Set_Player_Card(Player player, string player_card)
            {
                Card dealed_card;
                dealed_card = new Card();
                dealed_card.card = player_card;
                dealed_card.player_id = player.ID;
                player.Cards.Add(dealed_card);

                PossibleCards possible_card;
                possible_card = new PossibleCards();
                possible_card.card = dealed_card.card + " - (DEALT CARD AT BEGINING OF GAME)";
                possible_card.player_id = player.ID;
                player.PossibleCards.Add(possible_card);

                Card_Deck.Remove(player_card);
                Card_Deck.Shuffle_Deck();
            }

            public static void Begin_Game()
            {
                List<string> possible_seven = new List<string>();
                Card_Deck.Shuffle_Deck();
                var temp_card_list = Card_Deck.ToList();

                var combination = 1;
                int count = Card_Deck.Count();

                do
                {
                    int deal_card;
                    string hidden_card = "";

                    Random r = new Random();
                    deal_card = r.Next(0, Card_Deck.Count());
                    hidden_card = Card_Deck[deal_card];
                    possible_seven.Add(hidden_card);

                    var temp_card = temp_card_list[deal_card];
                    temp_card_list.Remove(temp_card);
                    Card_Deck.Remove(temp_card);

                    count = temp_card_list.Count();

                    foreach (var Card in Card_Deck.ToList())
                    {
                        Console.WriteLine("------------- COMBINATION - " + combination);
                        Console.WriteLine("-------------------------------");
                        possible_seven.Add(Card);

                        foreach (var player in Players_List)
                        {
                            foreach (var tc_item in Table_Cards)
                            {
                                PossibleCards possible_card = new PossibleCards();
                                possible_card.card = tc_item.card + " - (VISIBLE CARD ON TABLE)";
                                possible_card.player_id = player.ID;
                                player.PossibleCards.Add(possible_card);
                            }

                            foreach (var card in possible_seven)
                            {
                                PossibleCards possible_card = new PossibleCards();
                                possible_card.card = card + " - (HIDDEN CARD ON TABLE)";
                                possible_card.player_id = player.ID;
                                player.PossibleCards.Add(possible_card);
                            }

                            Console.WriteLine("POSSIBLE 7 CARDS FOR " + player.NAME);
                            Console.WriteLine();

                            foreach (var card in player.PossibleCards)
                            {
                                Console.WriteLine(card.card);
                            }

                            Check_Best_Hand(player);

                            var deletion_list = player.PossibleCards.Skip(2).ToList();
                            foreach (var pos_card in deletion_list)
                            {
                                player.PossibleCards.Remove(pos_card);
                            }

                            Console.WriteLine("-------------------------------");
                        }

                        possible_seven.Remove(Card);
                        Card_Deck.Shuffle_Deck();

                        Console.WriteLine("CARDS LEFT IN DECK - " + (temp_card_list.Count - 1));
                        Console.WriteLine("-------------------------------");
                        combination++;
                    }

                    possible_seven.Clear();
                    Card_Deck = temp_card_list;
                    Card_Deck.Shuffle_Deck();
                    count--;
                } while (count > 0);

                Console.ReadLine();
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

            public static void Check_Best_Hand(Player player)
            {
                var players_cards = player.PossibleCards.OrderByDescending(x => x.card.ToString()).ToList();

                var royal_list = players_cards.Where(x => x.card.Substring(1, 1) == "D").ToList();
                player.R_FLUSH = royal_list.Count < 5 ? false : royal_list.RoyalFlush(player);
            }
        }
    }
}
