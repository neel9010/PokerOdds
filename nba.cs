using System;
using System.Collections.Generic;

namespace NBA2020
{
    public class Team
    {
        public int NUM = 0;
        public string NAME = "";
        public int HOME_GAME = 0;
    }

    public class Round
    {
        public List<Team> Teams = new List<Team>();
        public int ROUND = 0;
    }

    public class Schedule
    {




        private static void Main(string[] args)
        {




            // Teams.Get_Random_Teams();
            Console.WriteLine("--------------------------------------------------------");

            //foreach (var team in Teams)
            //{
            //    Console.WriteLine(team.NUM + " - " + team.NAME);
            //}

            NBA.Start_Season();

            Console.Read();
        }
    }

    public class NBA
    {


        public static string Get_Teams(int team_number)
        {
            switch (team_number)
            {
                case 1: return "Atlanta Hawks         ";
                case 2: return "Boston Celtics        ";
                case 3: return "Brooklyn Nets         ";
                case 4: return "Charlotte Hornets     ";
                case 5: return "Chicago Bulls         ";
                case 6: return "Cleveland Cavaliers   ";
                case 7: return "Dallas Mavericks      ";
                case 8: return "Denver Nuggets        ";
                case 9: return "Detroit Pistons       ";
                case 10: return "Golden State Warriors ";
                case 11: return "Houston Rockets       ";
                case 12: return "Indiana Pacers        ";
                case 13: return "Los Angeles Clippers  ";
                case 14: return "Los Angeles Lakers    ";
                case 15: return "Memphis Grizzlies     ";
                case 16: return "Miami Heat            ";
                case 17: return "Milwaukee Bucks       ";
                case 18: return "Minnesota Timberwolves";
                case 19: return "New Orleans Pelicans  ";
                case 20: return "New York Knicks       ";
                case 21: return "Oklahoma City Thunder ";
                case 22: return "Orlando Magic         ";
                case 23: return "Philadelphia 76ers    ";
                case 24: return "Phoenix Suns          ";
                case 25: return "Portland Trail Blazers";
                case 26: return "Sacramento Kings      ";
                case 27: return "San Antonio Spurs     ";
                case 28: return "Toronto Raptors       ";
                case 29: return "Utah Jazz             ";
                case 30: return "Washington Wizards    ";
                default: return "";
            }
        }

        public static void Start_Season()
        {
            List<Team> Teams = new List<Team>();

            for (int i = 1; i < 31; i++)
            {
                Team Team = new Team();
                Team.NUM = i;
                Team.NAME = NBA.Get_Teams(i);
                Teams.Add(Team);
            }

            foreach (var team in Teams)
            {
                Console.WriteLine(team.NUM + " - " + team.NAME);
            }

            var tmp_list = Teams;
            var home_list = Teams;

         


            var odd_teams = new List<Team>();
            var even_teams = new List<Team>();
            var extra_teams = new List<Team>();
            var first_group = new List<Team>();
            var second_group = new List<Team>();

            int home_cnt = 1;
            int away_cnt = 1;
            int home = 0;
            int away = 29;
            for (int j = 1; j < 59; j++)
            {
                Console.WriteLine("--------Round " + " - " + j);
                if (j > 1)
                {
                    Shuffle_Teams.Reassign_Index(ref home_list, home_cnt);

                    home_list.Reverse();
                    Shuffle_Teams.Reassign_Index(ref home_list, away_cnt);

                    home_list = tmp_list;
                }



                for (int i = 0; i < 15; i++)
                {
                    var home_team = home_list[i];

                    home_list.Reverse();
                    var away_team = home_list[i];

                    home_list = tmp_list;

                    Console.WriteLine(home_team.NUM + " vs " + away_team.NUM);
                }

                Console.WriteLine();
            }


        }
    }

    public static class Shuffle_Teams
    {
        public static void Get_Random_Teams<T>(this List<T> list)
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

        public static void Reassign_Index<T>(ref List<T> list, int skip)
        {
            var deletion_list = new List<T>();
            var temp_list = list;

            for (int i = 0; i < skip; i++)
            {
                var item = list[i];
                deletion_list.Add(item);
            }

            foreach (var item in deletion_list)
            {
                list.Remove(item);
            }

            foreach (var item in deletion_list)
            {
                temp_list.Add(item);
            }

            list = temp_list;
        }
    }
}
