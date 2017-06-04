using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerWinning
{
    public class Player
    {
        public int ID;
        public string NAME = "";
        public int GAME_WON = 0;
        public bool S_FLUSH = false;
        public bool FOUR = false;
        public bool FLUSH = false;
        public bool STRAIGHT = false;
        public bool THREE = false;
        public bool TWO_PAIR = false;
        public bool PAIR = false;
        public bool HIGH_CARD = false;
        public List<Card> Cards = new List<Card>();
        public static List<Player> players { get; set; }
        public List<PossibleCards> PossibleCards = new List<PossibleCards>();
    }

    public class PossibleCards
    {
        public int player_id;
        public string card = "";
    }

    public class TableCards
    {
        public string card = "";
    }

    public class Card
    {
        public int player_id;
        public string card = "";
    }

    public static class Shuffle
    {
        public static void ShuffleList<T>(this IList<T> list)
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
    }

    public class Poker
    {
        private static string[] deck = new string[52];
        private static List<TableCards> table_cards = new List<TableCards>();
        private static List<Player> players = null;
        private static List<string> cards_list = new List<string>();

        private static void Main(string[] args)
        {
            Deck.Initialize(deck);

            Console.WriteLine("---------------- Welcome to game of Texas Hold'em Poker ----------------");
            Console.WriteLine();
            Console.WriteLine("Enter Number of players (Max 10): ");
            string players = Console.ReadLine();
            Console.WriteLine();

            StartGame.CreatePlayers(int.Parse(players));

            StartGame.deal_table_cards();
            StartGame.deal_players_cards();
            StartGame.begin_game();

            Console.Read();
        }

        public class Deck
        {
            private static string suit = "C";
            private static int suit_num = 1;
            private static int card_num = 0;

            internal static void Initialize(string[] deck)
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
                return i == 1 ? "A" : i == 10 ? "0" : i == 11 ? "J" : i == 12 ? "Q" : i == 13 ? "K" : i.ToString();
            }

            private static string set_suit(int suit_num)
            {
                return suit_num == 2 ? "S" : suit_num == 3 ? "D" : "H";
            }
        }

        public class StartGame
        {
            public string CARD_1 { get; set; }
            public string CARD_2 { get; set; }
            public string CARD_3 { get; set; }

            public static void CreatePlayers(int num)
            {
                for (int i = 0; i < num; i++)
                {
                    Player player = new Player();
                    player.ID = i;
                    player.NAME = SetPlayerName(i + 1).ToUpper();

                    if (players == null)
                    {
                        players = new List<Player>();
                    }

                    players.Add(player);
                }

                Console.WriteLine("Players For this game");
                foreach (var player in players)
                {
                    Console.WriteLine(player.NAME);
                }

                Console.WriteLine();

            }

            public static void deal_table_cards()
            {
                cards_list = deck.ToList();
                StartGame Game = new StartGame();
                string prev_card = "XX";
                for (int card_num = 1; card_num < 4; card_num++)
                {
                    int deal_card;
                    string table_card = null;

                    do
                    {
                        Random r = new Random();
                        var new_list = cards_list.Where(x => x != null).ToList();
                        deal_card = r.Next(0, new_list.Count());

                        table_card = new_list[deal_card];
                    } while (table_card == null);

                    prev_card = table_card;

                    TableCards tablecard = new TableCards();
                    tablecard.card = table_card;
                    table_cards.Add(tablecard);

                    cards_list.Remove(table_card);

                    Table_Cards(card_num, Game, table_card);
                }

                var list_count = cards_list.Count();

                Console.WriteLine("TOTAL CARDS IN DECK :" + deck.Length);
                Console.WriteLine("-------------------------------");
                Console.WriteLine("CARDS ON TABLE");
                Console.WriteLine("CARD 1 : " + Game.CARD_1);
                Console.WriteLine("CARD 2 : " + Game.CARD_2);
                Console.WriteLine("CARD 3 : " + Game.CARD_3);
                Console.WriteLine("-------------------------------");
                Console.WriteLine();
                Console.WriteLine("REMAINING CARDS AFTER TABLE DEAL :" + list_count);
                Console.WriteLine("-------------------------------");
            }

            public static void begin_game()
            {
                List<string> possible_seven = new List<string>();
                cards_list.ShuffleList();
                var temp_list = cards_list.ToList();

                var pos_count = 1;

                int count = cards_list.Count();
                do
                {
                    int deal_card;
                    string table_card = null;

                    Random r = new Random();
                    var new_list = cards_list.Where(x => x != null).ToList();
                    deal_card = r.Next(0, new_list.Count());

                    table_card = new_list[deal_card];

                    string first_card = table_card;
                    possible_seven.Add(table_card);

                    var tmp_itm = temp_list[deal_card];
                    temp_list.Remove(tmp_itm);
                    cards_list.Remove(tmp_itm);
                    count = temp_list.Count();
                    int player_count = players.Count();

                    foreach (var item in cards_list.ToList())
                    {
                        Console.WriteLine("------------- COMBINATION - " + pos_count);
                        Console.WriteLine("-------------------------------");
                        possible_seven.Add(item);
                        foreach (var player in players)
                        {
                            foreach (var tc_item in table_cards)
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

                            var list = player.PossibleCards.Skip(2).ToList();
                            foreach (var pos_card in list)
                            {
                                player.PossibleCards.Remove(pos_card);
                            }

                            player_count--;
                            Console.WriteLine("-------------------------------");
                        }

                        cards_list.ShuffleList();

                        possible_seven.Remove(item);

                        cards_list.ShuffleList();

                        Console.WriteLine("CARDS LEFT IN DECK - " + (temp_list.Count - 1));
                        Console.WriteLine("-------------------------------");
                        pos_count++;
                    }
                    possible_seven.Clear();
                    cards_list = temp_list;
                    cards_list.ShuffleList();

                    count--;
                } while (count > 0);

                Console.ReadLine();
            }

            public static void deal_players_cards()
            {
                foreach (var player in players)
                {
                    int deal_card;
                    string player_card = null;
                    Card dealed_card;
                    PossibleCards possible_card;
                    string prev_card = "XX";

                    for (int i = 1; i < 3; i++)
                    {
                        bool random_card = false;
                        do
                        {
                            Random r = new Random();
                            var new_list = cards_list.Where(x => x != null).ToList();

                            deal_card = r.Next(0, new_list.Count());
                            player_card = new_list[deal_card];

                            if (player.Cards == null)
                            {
                                player.Cards = new List<Card>();
                            }

                            var old_card = prev_card.Substring(1);
                            var new_card = player_card.Substring(1);

                            if (old_card != new_card)
                            {
                                dealed_card = new Card();
                                dealed_card.card = player_card;
                                dealed_card.player_id = player.ID;

                                possible_card = new PossibleCards();
                                possible_card.card = dealed_card.card + " - (DEALT CARD AT BEGINING OF GAME)";
                                possible_card.player_id = player.ID;
                                player.PossibleCards.Add(possible_card);

                                player.Cards.Add(dealed_card);

                                cards_list.Remove(player_card);

                                old_card = player_card;
                            }
                            else
                            {
                                random_card = true;
                                i = i - 1;
                            }
                        } while (player_card == null && !random_card);

                        prev_card = player_card;
                    }
                }

                foreach (var player in players)
                {
                    var list = player.Cards.Where(x => x.player_id == player.ID).ToList();
                    var card_count = 1;
                    Console.WriteLine("Dealt Cards for Player : " + player.NAME);
                    foreach (var card in list)
                    {
                        Console.WriteLine("CARD" + card_count + ":" + card.card);
                        card_count++;
                    }
                    Console.WriteLine("-------------------------------");
                }
                Console.WriteLine();
                Console.WriteLine("REMAINING CARDS AFTER PLAYER'S DEAL :" + cards_list.Count());
                Console.WriteLine("-------------------------------");
                Console.ReadLine();
            }

            public static void Table_Cards(int card_num, StartGame game, string card)
            {
                if (card_num == 1) { game.CARD_1 = card; } else if (card_num == 2) { game.CARD_2 = card; } else { game.CARD_3 = card; }
            }

            public static string SetPlayerName(int player_num)
            {
                if (player_num == 1)
                {
                    return "Neal";
                }
                else if (player_num == 2)
                {
                    return "Jasmin";
                }
                else if (player_num == 3)
                {
                    return "Gus";
                }
                else if (player_num == 4)
                {
                    return "Patric";
                }
                else if (player_num == 5)
                {
                    return "Mayme";
                }
                else if (player_num == 6)
                {
                    return "Kyong";
                }
                else if (player_num == 7)
                {
                    return "Lan";
                }
                else if (player_num == 8)
                {
                    return "Aldo";
                }
                else if (player_num == 9)
                {
                    return "Lan";
                }
                else
                {
                    return "Anisa";
                }
            }
        }
    }
}
