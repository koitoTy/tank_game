using server_client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Tank_v0._1_alpha;
using static Tank_v0._1_alpha.Program;

namespace Tank_v0._1_alpha
{
    class Program
    {
        private static IPAddress remoteIPAddress = IPAddress.Parse("127.0.0.1");// ай пи
        private static int remotePort = 8080;//порт вначале как логинсервер будет
        private static int localPort = 8080;//локальный порт        
        static List<PlayerState> Players = new List<PlayerState>();// игроки
        static List<WallState> Walls = new List<WallState>();//        

        //--------------------------------
        static string host = "localhost";
        //--------------------------------


        /* Тут должно быть получение координат с сервера и назначение их нашему танчику */

        static int Width = 100;/* Высота и ширина игрового поля */
        static int Height = 100;
        

      
        static void Print_tanks(int id, int x, int y, int dir)
        {
            PlayerState player = PlayerState.return_to_id(id, Players);
            Console.ForegroundColor = ConsoleColor.Black;
            PlayerState.WriteToLastPosition(player, "000\n000\n000");
            /*
                    000
                    000
                    000             
             */
            switch (id)
            {
                case 0: { Console.ForegroundColor = ConsoleColor.White; } break;
                case 1: { Console.ForegroundColor = ConsoleColor.Yellow; } break;
                case 2: { Console.ForegroundColor = ConsoleColor.Cyan; } break;
                case 3: { Console.ForegroundColor = ConsoleColor.Magenta; } break;
                case 4: { Console.ForegroundColor = ConsoleColor.Green; } break;
                case 5: { Console.ForegroundColor = ConsoleColor.Blue; } break;
                default: { Console.ForegroundColor = ConsoleColor.Yellow; }break;
            }

            PlayerState.NewPosition(player, x, y);

            switch (dir)
            {
                case 270:
                case 90: { Console.CursorLeft = x; Console.CursorTop = y; Console.Write("0 0\n000\n0 0"); } break;
                /*
                   0 0
                   000
                   0 0
                */
                case 180:
                case 0: { Console.CursorLeft = x; Console.CursorTop = y; Console.Write("000\n 0 \n000"); } break;
                    /*
                       000
                        0
                       000                
                    */
            }
        }

        static void Event_listener()
        {
            
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(remoteIPAddress, localPort);// new IPEndPoint(long.Parse(host), localPort);

            //receivingUdpClient.Connect(RemoteIpEndPoint);           

            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(RemoteIpEndPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                //Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    string returnData = builder.ToString();

                    //TYPEEVENT:ARG
                    string[] data_ = returnData.Split(':').ToArray<string>();

                    Task t = new Task(() => { Event_work(data_); }); t.Start();

                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }            
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
                th = false;//сбрасываем циклы приёма-передачи данных на сервер     
            }
        }

        static void Event_work(string[] Event)
        {
            for (int i = 0; i < Event.Length; i++)
            {
                string s = Event[i];
                s = Syntax.Trimming(s, '\0');

                Event[i] = s;
            }
            int ID = int.Parse(Event[1]),
                X = int.Parse(Event[2]),
                Y = int.Parse(Event[3]),
                DIR = int.Parse(Event[4]);

            switch (Event[0])
            {
                case "movetank":
                    {
                        Print_tanks(ID, X, Y, DIR);
                    }
                    break;
                case "createshot":
                    {

                        ShotState shot = new ShotState(new Position(X, Y), DIR, ID, int.Parse(Event[5]));
                        //Shots.Add(shot);                        
                        MoveShot(shot);
                    }
                    break;


                default: { return; } break;
            }
        }

        static void Event_to_server_tcp(string data_)// тестовая fn
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(remoteIPAddress, remotePort);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
                //Console.Write("Введите сообщение:");
                //string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(data_);
                socket.Send(data);
                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

        static void Receiv_Event_udp()// тестовая fn
        {

            IPEndPoint localIP = new IPEndPoint(remoteIPAddress, localPort);

            Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            listeningSocket.Bind(localIP);

            while (true)
            {
                // получаем сообщение
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байтов
                byte[] data = new byte[1024]; // буфер для получаемых данных

                //адрес, с которого пришли данные
                EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

                do
                {
                    bytes = listeningSocket.ReceiveFrom(data, ref remoteIp);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (listeningSocket.Available > 0);
                // получаем данные о подключении
                IPEndPoint remoteFullIp = remoteIp as IPEndPoint;

                
                //Console.WriteLine(Encoding.UTF8.GetString(data));

                // выводим сообщение
                Console.WriteLine("{0}:{1} - {2}", remoteFullIp.Address.ToString(),
                                                remoteFullIp.Port, builder.ToString());
            }
        }

        /// <summary>
        /// Перемещение снаряда
        /// </summary>
        /// <param name="shot">Снаряд</param>
        private static void MoveShot(ShotState shot)
        {
            ShotState Shot = shot;
            while ((!PlayerState.Return_hit_or_not(ShotState.ReturnShotPosition(Shot), ShotState.ReturnDamage(Shot))) &&
                (!WallState.Return_hit_or_not(ShotState.ReturnShotPosition(Shot), ShotState.ReturnDamage(Shot))))
            {
                // если достигли координат стен - то вернёт фалс и будет брейк
                ShotState.Position_plus_plus(Shot);
                ShotState.WriteShot(Shot);
            }
            Console.ForegroundColor = ConsoleColor.Black;//забываем путь полёта пули(закрашиваем его)
            ShotState.ForgetTheWay(Shot);
        }

        private static void from_to_key(object ob)
        {
            
            for_key = true;

            cooldown--;

            if (cooldown <= 0) { for_shot = true; cooldown = 10; }
            
        }

        static bool for_key = false;
        static bool for_shot = false;
        static int cooldown = 10;

        /* static void To_key()
         {
             //Приём нажатой клавиши           

             PlayerState MyTank = Players[MY_ID];

             System.Threading.Timer time = new System.Threading.Timer(new TimerCallback(from_to_key), null, 0, 10);

             while (true)
             {

                 //Console.CursorTop = 90; Console.CursorLeft = 90;
                 switch (Console.ReadKey().Key)
                 {
                     case ConsoleKey.Escape:
                         { time.Dispose(); th = false; break; }
                         break;

                     //пробел
                     case ConsoleKey.Spacebar:
                         {
                             if (for_shot)
                             {
                                 //"createshot"

                                 var shot = PlayerState.CreateShot(Players[MY_ID], 3);

                                 MessageToServer("createshot:" + PlayerState.ToString(MyTank) + ":3");// дамаг - 3
                                 var thr = new Task(() => { MoveShot(shot); });

                                 for_key = false;//откат кнопок
                                 for_shot = false;//откат выстрела
                             }
                         }
                         break;
                     case ConsoleKey.LeftArrow:
                         {
                             if (for_key)
                             {
                                 PlayerState.NewPosition_X(MyTank, '-');
                                 MessageToServer("movetank:" + PlayerState.ToString(MyTank)); for_key = false;
                             }
                         }
                         break;
                     case ConsoleKey.UpArrow:
                         {
                             if (for_key)
                             {
                                 PlayerState.NewPosition_Y(MyTank, '-');
                                 MessageToServer("movetank:" + PlayerState.ToString(MyTank)); for_key = false;
                             }
                         }
                         break;
                     case ConsoleKey.RightArrow:
                         {
                             if (for_key)
                             {
                                 PlayerState.NewPosition_X(MyTank, '+');
                                 MessageToServer("movetank:" + PlayerState.ToString(MyTank)); for_key = false;
                             }
                         }
                         break;
                     case ConsoleKey.DownArrow:
                         {
                             if (for_key)
                             {
                                 PlayerState.NewPosition_Y(MyTank, '+');
                                 MessageToServer("movetank:" + PlayerState.ToString(MyTank)); for_key = false;
                             }
                         }
                         break;


                     case ConsoleKey.PrintScreen:
                         { }
                         break;


                     case ConsoleKey.A:
                         {
                             if (for_key)
                             {
                                 PlayerState.NewPosition_X(MyTank, '-');
                                 MessageToServer("movetank:" + PlayerState.ToString(MyTank)); for_key = false;
                             }
                         }
                         break;

                     case ConsoleKey.D:
                         {
                             if (for_key)
                             {
                                 PlayerState.NewPosition_X(MyTank, '+');
                                 MessageToServer("movetank:" + PlayerState.ToString(MyTank)); for_key = false;
                             }
                         }
                         break;


                     // Аналог нажатия на пробел
                     case ConsoleKey.E:
                         {
                             if (for_shot)
                             {

                                 for_key = false;
                                 for_shot = false;
                             }
                         }
                         break;

                     // Аналог нажатия на пробел, но спец выстрел
                     case ConsoleKey.Q:
                         break;

                     case ConsoleKey.S:
                         {
                             if (for_key)
                             {
                                 PlayerState.NewPosition_Y(MyTank, '+');
                                 MessageToServer("movetank:" + PlayerState.ToString(MyTank)); for_key = false;
                             }
                         }
                         break;

                     case ConsoleKey.W:
                         {
                             if (for_key)
                             {
                                 PlayerState.NewPosition_Y(MyTank, '-');
                                 MessageToServer("movetank:" + PlayerState.ToString(MyTank)); for_key = false;
                             }
                         }
                         break;


                     case ConsoleKey.NumPad2:
                         {
                             if (for_key)
                             {
                                 PlayerState.NewPosition_Y(MyTank, '+');
                                 MessageToServer("movetank:" + PlayerState.ToString(MyTank)); for_key = false;                                
                             }
                         }
                         break;

                     case ConsoleKey.NumPad4:
                         {
                             if (for_key)
                             {
                                 PlayerState.NewPosition_X(MyTank, '-');
                                 MessageToServer(PlayerState.ToString(MyTank));                               
                             }
                         }
                         break;

                     case ConsoleKey.NumPad6:
                         {
                             if (for_key)
                             {
                                 PlayerState.NewPosition_X(MyTank, '+');
                                 MessageToServer("movetank:" + PlayerState.ToString(MyTank)); for_key = false;
                             }
                         }
                         break;

                     //нажатие на пробел
                     case ConsoleKey.NumPad7:
                         {
                             if (for_shot)
                             {

                                 for_key = false;
                                 for_shot = false;
                             }
                         }
                         break;
                     case ConsoleKey.NumPad8:
                         {
                             if (for_key)
                             {
                                 PlayerState.NewPosition_Y(MyTank, '-');
                                 MessageToServer("movetank:" + PlayerState.ToString(MyTank)); for_key = false;
                             }
                         }
                         break;

                     // Аналог нажатия на пробел но спец выстрел
                     case ConsoleKey.NumPad9:
                         break;


                     default:
                         break;

                 }
             }
         }*/


        static IPEndPoint ipPoint = new IPEndPoint(remoteIPAddress, remotePort);

        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static void MessageToServer(string data)//tcp
        {
            /*      Тут будет отправка сообщения на сервер     */
                       

            try
            {
                // Преобразуем данные в массив байтов
               /* IPEndPoint ipPoint = new IPEndPoint(remoteIPAddress, remotePort);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);*/
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);                
                byte[] data_ = Encoding.Unicode.GetBytes(data);
                socket.Send(data_);
                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
                th = false;
            }
            finally
            {
                // Закрыть соединение                
            }
        }
        
        
        // Структура стен
        public struct WallState
        {
            private Position Wall_block { get; set; }
            private int HP { get; set; }
            private static void hp_minus(WallState wall ,int damage)
            {
                wall.HP -= damage;
            }

            /// <summary>
            /// Создаём блок стены
            /// </summary>
            /// <param name="bloc">Координаты блока</param>
            /// <param name="hp">Здоровье</param>
            public WallState(Position bloc, int hp)
            {
                Wall_block = bloc; HP = hp;
            }
            public static bool Return_hit_or_not(Position pos, int damage)
            {
                if (pos.X <= 0 || pos.Y <= 0 || pos.X >= Width || pos.Y >= Height) { return true; }
                //
                //
                //
                for (int i = 0; i < Walls.Count; i++)
                {
                    if ((Walls[i].Wall_block.X == pos.X) && 
                        (Walls[i].Wall_block.Y == pos.Y))
                    {
                        WallState.hp_minus(Walls[i], damage);

                        if (Walls[i].HP <= 0)
                        {
                            Console.CursorLeft = pos.X; Console.CursorTop = pos.Y;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Walls.RemoveAt(i);
                            Console.Write("0");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        return true;
                    }
                }
                return false;
            }
        }

        // Структура для удобного представления выстрела
        public struct ShotState
        {
            /// <summary>
            /// Забываем путь(закрашиваем его)
            /// </summary>
            /// <param name="shot">Снаряд</param>
            public static void ForgetTheWay(ShotState shot)
            {
                int[] x = ShotState.x_way_array(shot);
                int[] y = ShotState.y_way_array(shot);

                switch (shot.dir)
                {
                    case 0: {
                            for (int i = 0; i < x.Length - 1; i++)
                            {
                                Console.CursorTop = y[0];
                                Console.CursorLeft = x[i];
                                Console.Write("0");
                            }
                        } break;
                    case 90: {
                            for (int i = 0; i < y.Length - 1; i++)
                            {
                                Console.CursorLeft = x[0];
                                Console.CursorTop = y[i];
                                Console.Write("0");
                            }
                        } break;
                    case 180: {
                            for (int i = 0; i < x.Length - 1; i++)
                            {
                                Console.CursorLeft = x[i];
                                Console.CursorTop = y[0];
                                Console.Write("0");
                            }
                        } break;
                    case 270: {
                            for (int i = 0; i < y.Length - 1; i++)
                            {
                                Console.CursorTop = y[i];
                                Console.CursorLeft = x[0];
                                Console.Write("0");
                            }
                        } break;
                }
            }

            /// <summary>
            /// Конструктор снарядов
            /// </summary>
            /// <param name="positionShot">Позиция выстрела</param>
            /// <param name="dir_">Куда летим</param>
            /// <param name="ID_Player">От кого летим</param>
            /// <param name="dam">Какой урон</param>
            public ShotState(Position positionShot, int dir_, int ID_Player_, int dam)
            {
                Shot_position = positionShot;
                dir = dir_;
                ID_Player = ID_Player_;
                damage = dam;

                x_way = new List<int>(); y_way = new List<int>();

                x_way.Add(Shot_position.X); y_way.Add(Shot_position.Y);
            }

            public static string To_string(ShotState shot)
            {
                return shot.ID_Player.ToString() + ":" + shot.Shot_position.X + ":"
                    + shot.Shot_position.Y + ":" + shot.dir + ":" + shot.damage;                
            }

            private Position Shot_position { get; set; }
            private int dir { get; set; }
            private int ID_Player { get; set; }
            private int damage { get; set; }

            private List<int> x_way { get; set; }
            private List<int> y_way { get; set; }

            private static int[] x_way_array(ShotState shot)
            {
                return shot.x_way.ToArray();
            }

            private static int[] y_way_array(ShotState shot)
            {
                return shot.y_way.ToArray();
            }

            public static void NewPosition(ShotState shot, int X, int Y)
            {
                shot.Shot_position.X = X;
                shot.Shot_position.Y = Y;
                shot.x_way.Add(shot.Shot_position.X); shot.y_way.Add(shot.Shot_position.Y);
            }

            public static void WriteShot(ShotState shot)
            {
                Console.CursorLeft = shot.Shot_position.X;
                Console.CursorTop = shot.Shot_position.Y;
                Console.Write("0");
            }

            public static void Position_plus_plus(ShotState shot)
            {
                switch (shot.dir)
                {
                    case 0: { shot.Shot_position.X += 1; } break;
                    case 90: { shot.Shot_position.Y -= 1; } break;
                    case 180: { shot.Shot_position.X -= 1; } break;
                    case 270: { shot.Shot_position.Y += 1; } break;
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorLeft = shot.Shot_position.X; Console.CursorTop = shot.Shot_position.Y;
                Console.Write("0");
                shot.x_way.Add(shot.Shot_position.X); shot.y_way.Add(shot.Shot_position.Y);
                //Array.Find && Array.Find
            }

            public static Position ReturnShotPosition(ShotState shot)
            {
                return shot.Shot_position;
            }

            public static int ReturnDamage(ShotState shot)
            {
                return shot.damage;
            }
        }

        // Класс для удобного представления координат
        public class Position
        {
            // Публичные свойства класса
            public int X { get; set; }
            public int Y { get; set; }


            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        // Структура состояние игрока
        public struct PlayerState
        {
            // Метод с названием идентичным названию класса или структуры
            // называется конструктор, он позволяет инициализировать свойства
            // и поля структуры или класса

            public static PlayerState return_to_id(int id,List<PlayerState> players)//САМАЯ ВАЖНАЯ FN
            {
                foreach (var item in players)
                {
                    if (item.ID == id) return item;
                } return players[0];
            }
            
            public static Position ReturnMyPosition(PlayerState player) { return player.Position; }
            public static int NewDir(PlayerState player, int dir) { player.dir = dir; return dir; }

            /// <summary>
            /// Конструктор структуры
            /// </summary>
            /// <param name="startPosition">Экземпляр позиции</param>
            /// <param name="id">ID</param>
            /// <param name="last">Последняя позиция игрока</param>
            /// <param name="dir_">Направление корпуса игрока (градусы)</param>
            /// <param name="key_">Нажатая кнопочка</param>                
            public PlayerState(Position startPosition, Position last, int dir_, int id, int Hp)
            {
                //это идентично записи if (ForExeption(startPosition) == true) { StartPosition = startPosition; } 

                Position = startPosition;
                LastPosition = last;
                dir = dir_;
                ID = id;
                hp = Hp;

                Collider_X = new int[3];
                Collider_Y = new int[3];

                Collider_Y[0] = startPosition.Y; Collider_Y[1] = startPosition.Y + 1; Collider_Y[2] = startPosition.Y + 2;
                Collider_X[0] = startPosition.X; Collider_X[1] = startPosition.X + 1; Collider_X[2] = startPosition.X + 2;
            }

            /// <summary>
            /// Возвращаем было ли касание кого-либо этим выстрелом (для удаления его)
            /// </summary>
            /// <returns></returns>
            public static bool Return_hit_or_not(Position pos, int damage)
            {
                if (pos.X <= 0 || pos.Y <= 0 || pos.X >= Width || pos.Y >= Height) { return true; }
                //

                //

                //                
                for (int i = 0; i < Players.Count; i++)
                {
                    if ((Array.IndexOf(Players[i].Collider_X, pos.X) != 0) && 
                        (Array.IndexOf(Players[i].Collider_Y, pos.Y) != 0))
                    {
                        hp_minus(Players[i], damage);
                        if (Players[i].hp > 0) { return true; }
                        else { Players.RemoveAt(i); return true; }
                    }                    
                }
                return false;
            }               
            

        /// <summary>
        /// Минус одно хп
        /// </summary>
        /// <param name="player">Игрок</param>
        /// <param name="hp">Сколько хп отнимаем</param>
        public static void hp_minus(PlayerState player, int hp)
        {
            player.hp -= hp;
        }

        /// <summary>
        /// Назначаем новую позицию по координате X
        /// </summary>
        /// <param name="player">Игрок</param>            
        public static int NewPosition_X(PlayerState player, char X)
        {
                if (((player.Position.X - 1 > 0) &&
                        (player.Position.X + 1 < Width))&&
                        !WallState.Return_hit_or_not(player.Position, 0))
                {
                    player.LastPosition = player.Position;
                    switch (X)
                    {
                        case '-': { player.Position.X -= 1; player.dir = 180; return 180; } break;
                        case '+': { player.Position.X += 1; player.dir = 0; return 0; } break;
                        default:
                            return player.dir;
                            break;
                    }
                }
                player.Collider_X = new int[3];                
                                
                player.Collider_X[0] = X; player.Collider_X[1] = X + 1; player.Collider_X[2] = X + 2;
                return player.dir;
        }

        /// <summary>
        /// Назначаем новую позицию по координате Y
        /// </summary>
        /// /// <param name="player">Игрок</param>            
        public static int NewPosition_Y(PlayerState player, char Y)
        {
            if (((player.Position.Y - 1 > 0) && 
                    (player.Position.Y + 1 < Height))&&
                       !WallState.Return_hit_or_not(player.Position, 0))
            {
                player.LastPosition = player.Position;
                switch (Y)
                {
                    case '-': { player.Position.Y -= 1; player.dir = 270; return 270; } break;
                    case '+': { player.Position.Y += 1; player.dir = 90; return 90; } break;
                    default: return player.dir;
                        break;
                }
            }                
                player.Collider_Y = new int[3];

                player.Collider_Y[0] = Y; player.Collider_Y[1] = Y + 1; player.Collider_Y[2] = Y + 2;                
                return player.dir;
        }

        /// <summary>
        /// Назначаем другим игрокам позиции
        /// </summary>
        /// <param name="player">Игрок</param>
        /// <param name="X">Координата X</param>
        /// <param name="Y">Координата Y</param>
        public static void NewPosition(PlayerState player, int X, int Y)
        {
            if ((X > 0 && X < Width) && (Y > 0 && Y < Height))
            {
                player.LastPosition = player.Position;
                player.Position.X = X;
                player.Position.Y = Y;

                    player.Collider_X = new int[3];
                    player.Collider_Y = new int[3];

                    player.Collider_Y[0] = Y; player.Collider_Y[1] = Y + 1; player.Collider_Y[2] = Y + 2;
                    player.Collider_X[0] = X; player.Collider_X[1] = X + 1; player.Collider_X[2] = X + 2;
            }
        }

        /// <summary>
        /// Печатает текст в предидущую позицию игрока
        /// </summary>
        /// <param name="player">Игрок</param>
        /// <param name="TEXT">Танчик</param>
        /// <returns></returns>
        public static void WriteToLastPosition(PlayerState player, string TEXT)
        {
           Console.ForegroundColor = ConsoleColor.Black;
           for (int i = player.Position.Y; i < player.LastPosition.Y + 3; i++)
           {
              for (int k = player.Position.X; k < player.LastPosition.X + 3; k++)
              {
                  Console.CursorTop = i; Console.CursorLeft = k;
                  Console.Write(TEXT);
              }
           }
           Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Вернёт нам текст в удобной для отправки форме
        /// </summary>
        /// <param name="player">Игрок</param>
        /// <returns></returns>
        public static string ToString(PlayerState player)
        {
            string argReturn =
                player.ID.ToString() + ":" + player.Position.X + ":" + player.Position.Y+":" + player.dir.ToString()+":"
                +player.LastPosition.X.ToString()+":"+player.LastPosition.Y.ToString();

            return argReturn;
        }
        private int ID { get; set; }
        public Position Position { get; set; }
        public Position LastPosition { get; set; }
        private int[] Collider_X { get; set; }// коллайдер
        private int[] Collider_Y { get; set; }
        private int hp { get; set; }
        //стартовое положение
        public int dir;

        private static bool ForExeption(Position startPosition)
        {
            if (startPosition.X > 0 && startPosition.Y > 0) return true;
            return false;
        }

        public static ShotState CreateShot(PlayerState player, int damage)
        {
                ShotState shot = new ShotState();
                switch (player.dir)
                {
                    case 0: {
                            Position pos = new Position(player.Position.X - 4, player.Position.Y - 1);
                            shot = new ShotState(pos, player.dir, player.ID, damage);
                    } break;
                    case 90: {
                            Position pos = new Position(player.Position.X + 1, player.Position.Y - 1);
                            shot = new ShotState(pos, player.dir, player.ID, damage);
                        } break;
                    case 180: {
                            Position pos = new Position(player.Position.X + 1, player.Position.Y + 1);
                            shot = new ShotState(pos, player.dir, player.ID, damage);
                        } break;
                    case 270: {
                            Position pos = new Position(player.Position.X + 1, player.Position.Y - 4);
                            shot = new ShotState(pos, player.dir, player.ID, damage);
                        } break;
                    default:
                        break;
                }                 

            return shot;
        }

        }


        static int MY_ID = 0;
        static bool th = true;
    }
}


namespace server_client
{
    public class net
    {
        static int MY_ID = 0;
        static List<PlayerState> Players = new List<PlayerState>();// игроки
        static List<WallState> Walls = new List<WallState>();//        

        //--------------------------------
        static string host = "localhost";
        //--------------------------------


        /* Тут должно быть получение координат с сервера и назначение их нашему танчику */

        static int Width = 100;/* Высота и ширина игрового поля */
        static int Height = 100;

        private static IPAddress remoteIPAddress = IPAddress.Parse("127.0.0.1");// ай пи
        private static int remotePort = 8080;//порт геймсервер
        private static int localPort = 22345;//локальный порт      
        //private static int WorkPort = 22543;

        static void Object_received()
        {
            try
            {
                Console.WriteLine("Получаю координаты объектов игры......");

                IPEndPoint ipPoint = new IPEndPoint(remoteIPAddress, localPort);

                // создаем сокет
                Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Ожидание...");
                /* 
                 *
                 *      Стены в виде X:Y:HP; X:Y:HP; X:Y:HP; 
                 *      
                 *      Игроки в виде ID:X:Y:HP:DIR; ID:X:Y:HP:DIR; 
                 *      
                 *      Порт - порт игры
                 *
                 */
                DataReceiv(listenSocket);

                listenSocket.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message + ex.Source); /*th = false;*/ }
        }

        private static void DataReceiv(Socket listenSocket)
        {
            Socket handler = listenSocket.Accept();
            // получаем сообщение


            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байтов
            byte[] data = new byte[256]; // буфер для получаемых данных
            do
            {
                bytes = handler.Receive(data);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
            }
            while (handler.Available > 0);
            handler.Shutdown(SocketShutdown.Both);

            
                string s = Syntax.Trimming(builder.ToString(), '\n');
                s = Syntax.Trimming(s, ' ');
                string[] data_ = s.Split('M').ToArray();

                int ui = int.Parse(data_[2]);

                string wall_ = data_[0], players_ = data_[1];

                remotePort = int.Parse(data_[3]);

                Console.WriteLine("Наш ID получен!");

                string[] str_wall = wall_.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                Console.WriteLine("Создаю стены.......");
            Console.Clear();
                foreach (string item in str_wall)
                {
                    string[] to_wall = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                    char[] TESTARRAY = to_wall[0].ToCharArray();
                    if (TESTARRAY.Length == 1)
                    {
                        int x = Convert.ToInt32(to_wall[0]); Console.CursorLeft = x;
                        int y = Convert.ToInt32(to_wall[1]); Console.CursorTop = y;
                        int hp = Convert.ToInt32(to_wall[2]); Console.Write("0");

                        var position = new Position(x, y);
                        var wall = new WallState(position, hp);
                        Walls.Add(wall);
                    }
                    else
                    {
                        int x = Convert.ToInt32(TESTARRAY[1].ToString()); Console.CursorLeft = x;
                        int y = Convert.ToInt32(to_wall[1]); Console.CursorTop = y;
                        int hp = Convert.ToInt32(to_wall[2]); Console.Write("0");

                        var position = new Position(x, y);
                        var wall = new WallState(position, hp);
                        Walls.Add(wall);
                    }
                }
                
                string[] play = players_.Split(';').ToArray();
                int k = 0;           
                do
                {

                    string[] to_play = play[k].Split(':').ToArray();
                    char[] TESTARRAY = to_play[0].ToCharArray();
                    if (TESTARRAY.Length == 1)
                    {
                        int ID = int.Parse(to_play[0]);
                        Position pos = new Position(int.Parse(to_play[1]), int.Parse(to_play[2]));
                        int hp = int.Parse(to_play[3]);
                        int dir = int.Parse(to_play[4]);

                        PlayerState player = new PlayerState(pos, pos, dir, ID, hp);
                        Players.Add(player);
                    }
                    else
                    {
                        int ID = Convert.ToInt32(TESTARRAY[1]);
                        Position pos = new Position(int.Parse(to_play[1]), int.Parse(to_play[2]));
                        int hp = int.Parse(to_play[3]);
                        int dir = int.Parse(to_play[4]);

                        PlayerState player = new PlayerState(pos, pos, dir, ID, hp);
                        Players.Add(player);
                    }
                    k += 1;
                } while (k < ui);

            ipPoint = new IPEndPoint(remoteIPAddress, remotePort);
                handler.Close();            
        }

        static void send()
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(remoteIPAddress, remotePort);
                
                // подключаемся к удаленному хосту
                socketReceiv.Connect(ipPoint);
                //Console.Write("Введите сообщение:");
                string message = "000";
                byte[] data = Encoding.UTF8.GetBytes(message);
                socketReceiv.Send(data);
                IPEndPoint s = (IPEndPoint)socketReceiv.LocalEndPoint;
                localPort = s.Port;
                // закрываем сокет
                socketReceiv.Shutdown(SocketShutdown.Send);
                socketReceiv.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.Source);
            }
        }

        static bool proofToWork = true;//если ложь - разрешаем печатать и что-либо менять
        static bool endToWork = true;//если истина - то это значит что мы напечатали/что-то изменила

        static void PrintCoordinate(int x, int y, int dir, int id)
        {
            while(true)                
            {
                if (proofToWork)
                {
                    endToWork = false;
                                     
                    Console.CursorLeft = x; Console.CursorTop = y;
                    switch (dir)
                    {
                        case 90:
                        case 270: {
                                Console.Write("0 0");
                                Console.CursorLeft = x; Console.CursorTop = y + 1;
                                Console.Write("000");
                                Console.CursorLeft = x; Console.CursorTop = y + 2;
                                Console.Write("0 0");
                        } break;
                        case 0:
                        case 180: {
                                Console.Write("000");
                                Console.CursorLeft = x; Console.CursorTop = y + 1;
                                Console.Write(" 0 ");
                                Console.CursorLeft = x; Console.CursorTop = y + 2;
                                Console.Write("000");
                        } break;
                        default: { } break;
                    }
                    
                    endToWork = true;
                    
                    break;
                }
                else { }
            }  
        }

        public static void Connect()
        {
            send();
            Object_received();
            socket.Connect(ipPoint);
            MessageToServer("sender:delete_me");                       
        }       

        private static ConsoleKey ToKey()
        {
            ConsoleKey ret = Console.ReadKey().Key;
            return ret;
        }
        static bool end = true;

        private static void LastPosotionPrint(ConsoleColor colorStart,int x, int y, string TEXT, ConsoleColor colorEnd)
        {
            Console.ForegroundColor = colorStart;

            Console.CursorLeft = x; Console.CursorTop = y; Console.Write(TEXT);
            Console.CursorLeft = x+1; Console.CursorTop = y; Console.Write(TEXT);
            Console.CursorLeft = x+1; Console.CursorTop = y+1; Console.Write(TEXT);
            Console.CursorLeft = x+1; Console.CursorTop = y-1; Console.Write(TEXT);
            Console.CursorLeft = x-1; Console.CursorTop = y+1; Console.Write(TEXT);
            Console.CursorLeft = x-1; Console.CursorTop = y-1; Console.Write(TEXT);
            Console.CursorLeft = x-2; Console.CursorTop = y; Console.Write(TEXT);
            Console.CursorLeft = x-2; Console.CursorTop = y+2; Console.Write(TEXT);
            Console.CursorLeft = x-2; Console.CursorTop = y-2; Console.Write(TEXT);
            Console.CursorLeft = x+2; Console.CursorTop = y; Console.Write(TEXT);
            Console.CursorLeft = x+2; Console.CursorTop = y+2; Console.Write(TEXT);
            Console.CursorLeft = x+2; Console.CursorTop = y-2; Console.Write(TEXT);

            Console.ForegroundColor = colorEnd;
        }

        static void Work()
        {
            PlayerState MyTank = PlayerState.return_to_id(MY_ID, Players);
            Task t = new Task(() => {/* while (end)*/ Eventlistener(); }); t.Start();
            while (true)
            {                
                switch (ToKey())
                {
                    case ConsoleKey.DownArrow: {
                            if (endToWork) { /* Код пишем тут  */
                                MyTank.dir = 270;                                

                                Position last = MyTank.Position;
                                int x = last.X; int y = last.Y;
                                MyTank.LastPosition = new Position(x, y);
                                MyTank.Position.Y += 1;
                                //PrintCoordinate(p.X, p.Y, MyTank.dir, MY_ID);
                                MessageToServer("movetank:"+PlayerState.ToString(MyTank));
                            }
                    } break;
                    case ConsoleKey.UpArrow: {
                            if (endToWork) { /* Код пишем тут  */
                                MyTank.dir = 90;
                                
                                Position last = MyTank.Position;
                                int x = last.X; int y = last.Y;
                                MyTank.LastPosition = new Position(x, y);
                                MyTank.Position.Y -= 1;                                
                                //PrintCoordinate(p.X, p.Y, MyTank.dir, MY_ID);
                                MessageToServer("movetank:" + PlayerState.ToString(MyTank));
                            }

                    } break;
                    case ConsoleKey.LeftArrow: {
                            if (endToWork) { /* Код пишем тут  */
                                MyTank.dir = 180;
                                
                                Position last = MyTank.Position;
                                int x = last.X; int y = last.Y;
                                MyTank.LastPosition = new Position(x, y);
                                MyTank.Position.X -= 1;                                
                                //PrintCoordinate(p.X, p.Y, MyTank.dir, MY_ID);
                                MessageToServer("movetank:" + PlayerState.ToString(MyTank));
                            } } break;
                    case ConsoleKey.RightArrow: {
                            if (endToWork) { /* Код пишем тут  */
                                MyTank.dir = 0;
                                
                                Position last = PlayerState.ReturnMyPosition(MyTank);
                                int x = last.X; int y = last.Y;
                                MyTank.LastPosition = new Position(x, y);
                                MyTank.Position.X += 1;                                
                                //PrintCoordinate(p.X, p.Y, MyTank.dir, MY_ID);
                                MessageToServer("movetank:" + PlayerState.ToString(MyTank));
                            } } break;
                    case ConsoleKey.Spacebar: { if (endToWork)
                            {
                                int x = 0, y = 0;
                                switch (MyTank.dir)
                                {
                                    /*  меняется положение в зависимости от направления игрока  */
                                    case 0: { x = MyTank.Position.X + 1; y = MyTank.Position.Y + 1;  } break;
                                    case 180: { x = MyTank.Position.X - 4; y = MyTank.Position.Y + 1; } break;
                                    case 90: { x = MyTank.Position.X + 1; y = MyTank.Position.Y - 1; } break;
                                    case 270: { x = MyTank.Position.X + 1; y = MyTank.Position.Y + 1; } break;
                                    default:
                                        break;
                                }
                                Position p;
                                if (x > 0 && y > 0)
                                {
                                    p = new Position(x, y);                                    
                                } else { p = MyTank.Position; }

                                ShotState shot = new ShotState(p, MyTank.dir, MY_ID, 5);

                                MessageToServer("createshot:"+ShotState.To_string(shot));
                            } }break;
                    case ConsoleKey.Escape: { end = false; return; } break;
                    default: { } break;
                }
            }
        }

        static void Eventlistener()
        {

            IPEndPoint RemoteIpEndPoint = new IPEndPoint(remoteIPAddress, localPort);// new IPEndPoint(long.Parse(host), localPort);

            //receivingUdpClient.Connect(RemoteIpEndPoint);           

            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(RemoteIpEndPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                //Console.WriteLine("Сервер запущен. Ожидание подключений...");
                while (end)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[16]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    string returnData = builder.ToString();

                    //TYPEEVENT:ARG
                    string[] data_ = returnData.Split(':').ToArray<string>();

                    Task t = new Task(() => { EventWork(data_); }); t.Start();
                }
                    // закрываем сокет                                  
            }
            catch (Exception ex)
            {
                Console.Clear();
                end = false;
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
                //th = false;//сбрасываем циклы приёма-передачи данных на сервер     
            }
        }

        static void EventWork(string[] Event)
        {
            for (int i = 0; i < Event.Length; i++)
            {
                string s = Event[i];
                s = Syntax.Trimming(s, '\0');

                Event[i] = s;
            }
            try
            {
               
            
            switch (Event[0])
            {
                case "movetank":
                    {
                            int ID = int.Parse(Event[1]),
                                   X = int.Parse(Event[2]),
                                   Y = int.Parse(Event[3]),
                                   DIR = int.Parse(Event[4]),
                                   LastX = int.Parse(Event[5]),
                                   LastY = int.Parse(Event[6]);
                            var player = PlayerState.return_to_id(ID, Players);
                        while (true)
                        {
                            if (proofToWork)
                            {
                                endToWork = false;
                                player.LastPosition.X = LastX; player.LastPosition.Y = LastY;
                                PlayerState.WriteToLastPosition(player, "000");
                                endToWork = true;
                                break;
                            }
                        } player.Position.X = X; player.Position.Y = Y; PrintCoordinate(X, Y, DIR, ID);
                    }
                    break;
                case "createshot":
                    {
                            int ID = int.Parse(Event[1]),
                                       X = int.Parse(Event[2]),
                                       Y = int.Parse(Event[3]),
                                       DIR = int.Parse(Event[4]);
                            ShotState shot = new ShotState(new Position(X, Y), DIR, ID, int.Parse(Event[5]));                        
                        MoveShot(shot);                       
                    }
                    break;


                default: { return; } break;
            }
            }
            catch (Exception ex) { Console.Clear(); Console.WriteLine("Возникла ошибка ->"+ex.Message+ex.Source+" Выключаюсь....."); end = false; Disconnect(); }
        }

        private static void MoveShot(ShotState shot)
        {
            ShotState Shot = shot;
            while ((!PlayerState.Return_hit_or_not(ShotState.ReturnShotPosition(Shot), ShotState.ReturnDamage(Shot))) &&
                (!WallState.Return_hit_or_not(ShotState.ReturnShotPosition(Shot), ShotState.ReturnDamage(Shot))))
            {
                // если достигли координат стен - то вернёт фалс и будет брейк
                ShotState.Position_plus_plus(Shot);
                if (endToWork) { proofToWork = false; ShotState.WriteShot(Shot); proofToWork = true; }
            }
            while (true)
            {
                if (endToWork)
                {
                    proofToWork = false;
                    Console.ForegroundColor = ConsoleColor.Black;//забываем путь полёта пули(закрашиваем его)
                    ShotState.ForgetTheWay(Shot);
                    proofToWork = true;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;                    
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("\n\n\n******** Введите IP сервера ********"); Console.Write("IP: ");
            remoteIPAddress = IPAddress.Parse(Console.ReadLine());
            Console.Write("\n-----------------------------\nВведите логин-порт сервера: ");
            remotePort = int.Parse(Console.ReadLine());
            Console.WriteLine("Начинаю получение объектов игры........");

            Connect();
            

            //Console.Clear();
            Work();
            
            Console.Clear();

            Console.WriteLine("Игра завершилась.....");
            Disconnect();
            Console.ReadLine();
        }


        static IPEndPoint ipPoint = new IPEndPoint(remoteIPAddress, remotePort);
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static IPEndPoint ipPoint_ = new IPEndPoint(remoteIPAddress, localPort);
        static Socket socketReceiv = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Disconnect()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();                
            }
            catch {  } finally { Console.Clear(); }
            end = false;
        }

        static void MessageToServer(string data)//tcp
        {
            /*      Тут будет отправка сообщения на сервер     */           

            try
            {
                byte[] data_ = Encoding.UTF8.GetBytes(data);
                socket.Send(data_);                                         
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
                Disconnect();
            }
            finally
            {
                // Закрыть соединение                
            }
        }
    }
}