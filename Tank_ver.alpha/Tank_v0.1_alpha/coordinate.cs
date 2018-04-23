using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace coordinate
{
    public class coordinate 
    {   
        private int count_coordinates { get; set; }

        /// <summary>
        /// Прибавляем к каждому элементу массива координат нужное число
        /// </summary>
        /// <param name="c_">Объект класса</param>
        /// <param name="number_coor">Номер координат</param>
        /// <param name="arg_plus">Сколько прибавляем</param>
        /// <returns>Объект класса</returns>
        internal coordinate all_plus(coordinate c_, int number_coor, int arg_plus)
        {            
            for (int i = 0; i < c_.list_coordinate[number_coor].Length; i++)
            {
                c_.list_coordinate[number_coor][i] =
                           c_.list_coordinate[number_coor][i] + arg_plus;
            }
            return c_;
        }

        /// <summary>
        /// Назначаем новые координаты и возвращаем успешность изменения
        /// </summary>
        /// <param name="number_">Номер старых координат</param>
        /// <param name="coor_">Новые координаты</param>
        /// <param name="name_">Новое имя координат</param>
        /// <returns></returns>
        internal bool new_coordinate(int number_, int[] coor_, string name_)
        {
            if (number_ >= count_coordinates)
                return false;
            list_coordinate[number_] = coor_;
            name_coordinates[number_] = name_;
            return true;
        }

        /// <summary>
        /// Назначаем новые координаты и возвращаем успешность изменения
        /// </summary>
        /// <param name="number_">Номер старых координат</param>
        /// <param name="coor_">Новые координаты</param>
        /// <returns></returns>
        internal bool new_coordinate(int number_, int[] coor_)
        {
            if (number_ >= count_coordinates)
                return false;
            list_coordinate[number_] = coor_;
            return true;
        }

        /// <summary>
        /// Количество всех координат
        /// </summary>
        /// <returns></returns>
        internal int count() { return count_coordinates; }

        /// <summary>
        /// Сравнение равенства координат
        /// </summary>
        /// <param name="eq1">Первые координаты</param>
        /// <param name="eq2">Вторые координаты</param>
        /// <returns></returns>
        internal bool equals(int[] eq1, int[] eq2)
        {
            if (eq1 == eq2)
                return true;
            return false;
        }
        
        /// <summary>
        /// Сравнение равенства координат по индексу
        /// </summary>
        /// <param name="number">Индекс координат 1</param>
        /// <param name="number_">Индекс координат 2</param>
        /// <returns>TRUE если равны, FALSE если нет</returns>
        internal bool equals(int number, int number_)
        {
            if (get_coordinate(number) == get_coordinate(number_))
                return true;
            return false;
        }

        /// <summary>
        /// Очистка всех нулевых координат|int[0]|null
        /// </summary>
        /// <returns></returns>
        internal int clear_null()
        {
            int k = 0;
            for (int i = 0; i < list_coordinate.Count; i++)
            {
                if(list_coordinate[i].Length == 0 || list_coordinate[i] == null)
                {
                    list_coordinate.RemoveAt(i);
                    name_coordinates.RemoveAt(i);
                    k += 1;
                    count_coordinates -= 1;
                }
            } return k;
        }
        
        /// <summary>
        /// Копирует координаты по индексу и добавляет в список
        /// </summary>
        /// <param name="number">Индекс</param>
        internal void copy_and_add(int number)
        {
            list_coordinate.Add(get_coordinate(number));
            name_coordinates.Add("");
            count_coordinates += 1;
        }
        /// <summary>
        /// Копирует координаты по индексу и добавляет в список
        /// </summary>
        /// <param name="number">Индекс</param>
        /// <param name="name">Имя новых координат</param>
        internal void copy_and_add(int number, string name)
        {
            list_coordinate.Add(get_coordinate(number));
            name_coordinates.Add(name);
            count_coordinates += 1;
        }
        /// <summary>
        /// Копирует координаты по имени и добавляет в список
        /// </summary>
        /// <param name="name_">Имя координат</param>
        internal void copy_and_add(string name_)
        {
            list_coordinate.Add(get_coordinate(name_));
            name_coordinates.Add("");
            count_coordinates += 1;
        }
        /// <summary>
        /// Копирует координаты по имени и добавляет в список
        /// </summary>
        /// <param name="name_">Имя координат</param>
        /// <param name="name">Имя будущих координат</param>
        internal void copy_and_add(string name_, string name)
        {
            list_coordinate.Add(get_coordinate(name_));
            name_coordinates.Add(name);
            count_coordinates += 1;
        }

        internal int[] get_coordinate(int number) { return list_coordinate[number]; }
        internal int[] get_coordinate(string name)
        {
            for (int i = 0; i < list_coordinate.Count; i++)
            {

                if (name_coordinates[i] == name)
                    return list_coordinate[i];
            }
            return new int[0];
        }

        private List<int[]> list_coordinate { get; set; }
        private List<string> name_coordinates { get; set; }


        internal void add(int[] array)
        {
            list_coordinate.Add(array);
            name_coordinates.Add("");
            count_coordinates += 1;
        }
        internal void add(int[] array, string name)
        {
            list_coordinate.Add(array);
            name_coordinates.Add(name);
            count_coordinates += 1;
        }
        internal void add(int x, int y)
        {
            if (x > y)
            {
                int[] coord = new int[x - y + 1];
                for (int i = y, k = 0; i < x + 1; i++, k++)
                {
                    coord[k] = i;
                }
//                list_coordinate = new List<int[]>();
//                name_coordinates = new List<string>();
                list_coordinate.Add(coord);
                name_coordinates.Add("");
                count_coordinates = list_coordinate.Count;
            }
            else if (y > x)
            {
                int[] coord = new int[y - x + 1];
                for (int i = x, k = 0; i < y + 1; i++, k++)
                {
                    coord[k] = i;
                }
//                list_coordinate = new List<int[]>();
//                name_coordinates = new List<string>();
                list_coordinate.Add(coord);
                name_coordinates.Add("");
                count_coordinates = list_coordinate.Count;
            }
            else
            {
                int[] coord = new int[1];
                coord[0] = y;
//                list_coordinate = new List<int[]>();
//                name_coordinates = new List<string>();
                list_coordinate.Add(coord);
                name_coordinates.Add("");
                count_coordinates = list_coordinate.Count;
            }
        }
        internal void add(int x, int y, string name)
        {
            if (x > y)
            {
                int[] coord = new int[x - y + 1];
                for (int i = y, k = 0; i < x + 1; i++, k++)
                {
                    coord[k] = i;
                }
//                list_coordinate = new List<int[]>();
//                name_coordinates = new List<string>();
                list_coordinate.Add(coord);
                name_coordinates.Add(name);
                count_coordinates = list_coordinate.Count;
            }
            else if (y > x)
            {
                int[] coord = new int[y - x + 1];
                for (int i = x, k = 0; i < y + 1; i++, k++)
                {
                    coord[k] = i;
                }
//                list_coordinate = new List<int[]>();
//                name_coordinates = new List<string>();
                list_coordinate.Add(coord);
                name_coordinates.Add(name);
                count_coordinates = list_coordinate.Count;
            }
            else
            {
                int[] coord = new int[1];
                coord[0] = y;
//                list_coordinate = new List<int[]>();
//                name_coordinates = new List<string>();
                list_coordinate.Add(coord);
                name_coordinates.Add(name);
                count_coordinates = list_coordinate.Count;
            }
        }

        internal void remove(int number)
        { list_coordinate.RemoveAt(number); name_coordinates.RemoveAt(number); }
        internal void remove(string name)
        {
            for (int i = 0; i < list_coordinate.Count; i++)
            {
                if (name_coordinates[i] == name)
                { list_coordinate.RemoveAt(i); name_coordinates.RemoveAt(i); }
            }            
        }

        internal int get_number(string name) 
        {
            for (int i = 0; i < list_coordinate.Count; i++)
            {
                if (name_coordinates[i] == name)
                     return i;
            } return -1;
        }
        internal int get_number(int[] coord_) 
        {
            for (int i = 0; i < list_coordinate.Count; i++)
            {
                if (list_coordinate[i] == coord_)
                     return i;                
            } return -1;
        }
        /// <summary>
        /// Расчет координат от одного числа, до другого
        /// </summary>
        /// <param name="x">Первое число</param>
        /// <param name="y">Второе число</param>
        public coordinate(int x, int y) 
        {
            if (x > y) 
            {
                int[] coord = new int[x - y + 1];
                for (int i = y, k = 0; i < x + 1; i++, k++)
                {
                    coord[k] = i;
                }
                list_coordinate = new List<int[]>();
                name_coordinates = new List<string>();
                list_coordinate.Add(coord);
                name_coordinates.Add("");
                count_coordinates = list_coordinate.Count;
            }
            else if (y > x)
            {
                int[] coord = new int[y - x + 1];
                for (int i = x, k = 0; i < y + 1; i++, k++)
                {
                    coord[k] = i;
                }
                list_coordinate = new List<int[]>();
                name_coordinates = new List<string>();
                list_coordinate.Add(coord);
                name_coordinates.Add("");
                count_coordinates = list_coordinate.Count;
            }
            else 
            {
                int[] coord = new int[1];
                coord[0] = y;
                list_coordinate = new List<int[]>();
                name_coordinates = new List<string>();
                list_coordinate.Add(coord);
                name_coordinates.Add("");
                count_coordinates = list_coordinate.Count;
            }
        }
        /// <summary>
        /// Расчет координат от одного числа, до другого
        /// </summary>
        /// <param name="x">Первое число</param>
        /// <param name="y">Второе число</param>
        /// <param name="name">Имя новых координат</param>
        public coordinate(int x, int y, string name)
        {
            if (x > y)
            {
                int[] coord = new int[x - y + 1];
                for (int i = y, k = 0; i < x + 1; i++, k++)
                {
                    coord[k] = i;
                }
                list_coordinate = new List<int[]>();
                name_coordinates = new List<string>();
                list_coordinate.Add(coord);
                name_coordinates.Add(name);
                count_coordinates = list_coordinate.Count;
            }
            else if (y > x)
            {
                int[] coord = new int[y - x + 1];
                for (int i = x, k = 0; i < y + 1; i++, k++)
                {
                    coord[k] = i;
                }
                list_coordinate = new List<int[]>();
                name_coordinates = new List<string>();
                list_coordinate.Add(coord);
                name_coordinates.Add(name);
                count_coordinates = list_coordinate.Count;
            }
            else
            {
                int[] coord = new int[1];
                coord[0] = y;
                list_coordinate = new List<int[]>();
                name_coordinates = new List<string>();
                list_coordinate.Add(coord);
                name_coordinates.Add(name);
                count_coordinates = list_coordinate.Count;
            }
        }
        /// <summary>
        /// Создание координат из существующего массива
        /// </summary>
        /// <param name="array">Массив</param>
        public coordinate(int[] array)
        {
            list_coordinate = new List<int[]>();
            name_coordinates = new List<string>();
            list_coordinate.Add(array);
            name_coordinates.Add("");
            count_coordinates = list_coordinate.Count;
        }
        /// <summary>
        /// Создание координат из существующего массива
        /// </summary>
        /// <param name="array">Массив</param>
        /// <param name="name">Имя новых координат</param>
        public coordinate(int[] array, string name)
        {
            list_coordinate = new List<int[]>();
            name_coordinates = new List<string>();
            list_coordinate.Add(array);
            name_coordinates.Add(name);
            count_coordinates = list_coordinate.Count;
        }
    }
    public class work
    {
        private coordinate coordinate_pb;
        private PictureBox pictureBox1 { get; set; }
        public work(PictureBox pb_)
        {
            pictureBox1 = pb_;            
        }

        /// <summary>
        /// Изменение привязанного pictureBox-а
        /// </summary>
        /// <param name="pb_">Новый pictureBox</param>
        internal void update_pb(PictureBox pb_)
        {
            pictureBox1 = pb_;
        }

        /// <summary>
        /// Пересекаются ли наши координаты с соседними снизу
        /// </summary>
        /// <param name="c_top">Нижние координаты</param>
        /// <returns></returns>
        internal bool eql_bottom(coordinate c_top)
        {
            int[] mine_t_x; int[] mine_t_y;
            int[] c_b_x; int[] c_b_y;
            bool x = false; //bool y = false;

            mine_t_x = coordinate_pb.get_coordinate(3);
            mine_t_y = coordinate_pb.get_coordinate(7);
            c_b_x = c_top.get_coordinate(1);
            c_b_y = c_top.get_coordinate(5);

            if (c_b_x.Length > mine_t_x.Length)
            {
                for (int i = 0; i < mine_t_x.Length; i++)
                {
                    if (mine_t_x[i] == c_b_x[i]) { x = true; }
                }
            }
            else
            {
                for (int i = 0; i < c_b_x.Length; i++)
                {
                    if (mine_t_x[i] == c_b_x[i]) { x = true; }
                }
            }
            if (c_b_y[0] == mine_t_y[0] && x) { return true; }
            return false;
        }

        /// <summary>
        /// Пересекаются ли наши координаты с соседними сверху
        /// </summary>
        /// <param name="c_top">Верхние координаты</param>
        /// <returns></returns>
        internal bool eql_top(coordinate c_top)
        {
            int[] mine_t_x; int[] mine_t_y;
            int[] c_b_x; int[] c_b_y;
            bool x = false; //bool y = false;

            mine_t_x = coordinate_pb.get_coordinate(1);
            mine_t_y = coordinate_pb.get_coordinate(5);
            c_b_x = c_top.get_coordinate(3);
            c_b_y = c_top.get_coordinate(7);

            if(c_b_x.Length > mine_t_x.Length)
            {
                for (int i = 0; i < mine_t_x.Length; i++)
                {
                    if (mine_t_x[i] == c_b_x[i]) { x = true; }
                }
            }
            else
            {
                for (int i = 0; i < c_b_x.Length; i++)
                {
                    if (mine_t_x[i] == c_b_x[i]) { x = true; }
                }
            }
            if (c_b_y[0] == mine_t_y[0] && x) { return true; }
            return false;
        }

        /// <summary>
        /// Пересекаются ли наши координаты с соседними слева 
        /// </summary>
        /// <param name="c_right">Координаты левого объекта</param>
        /// <returns></returns>
        internal bool eql_right(coordinate c_right)
        {
            // x or y
            // coordinate_pb - наши координаты (координаты объекта)
            // 4 - левый у, 6 - правый
            // 0 - левый Х, 2 - правый
            int[] x_mine; int[] y_mine; int[] x_r; int[] y_r;
            bool y = false; 
            //bool x = false;
            x_r = c_right.get_coordinate(0);
            x_mine = coordinate_pb.get_coordinate(2);
            y_mine = coordinate_pb.get_coordinate(6);
            y_r = c_right.get_coordinate(4);
            
            if (y_r.Length > y_mine.Length)
            {
                for (int i = 0; i < y_mine.Length; i++)
                {
                    if (y_mine[i] == y_r[i]) { y = true; }
                }
            }
            else
            {
                for (int i = 0; i < y_r.Length; i++)
                {
                    if (y_r[i] == y_mine[i]) { y = true; }
                }
            } if(x_mine[0] == x_r[0] && y) { return true; }
            return false;
        }

        /// <summary>
        /// Пересекаются ли наши координаты с соседними слева 
        /// </summary>
        /// <param name="c_left">Координаты левого объекта</param>
        /// <returns></returns>
        internal bool eql_left(coordinate c_left)
        {
            // x or y
            // coordinate_pb - наши координаты (координаты объекта)
            // 4 - левый у, 6 - правый
            // 0 - левый Х, 2 - правый
            int[] x_mine; int[] y_mine; int[] x_r; int[] y_r;
            bool y = false;
            //bool x = false;
            x_r = c_left.get_coordinate(2);
            x_mine = coordinate_pb.get_coordinate(0);
            y_mine = coordinate_pb.get_coordinate(4);
            y_r = c_left.get_coordinate(6);
            if (y_r.Length > y_mine.Length)
            {
                for (int i = 0; i < y_mine.Length; i++)
                {
                    if(y_mine[i] == y_r[i]) { y = true; }
                }
            }
            else
            {
                for (int i = 0; i < y_r.Length; i++)
                {
                    if (y_r[i] == y_mine[i]) { y = true; }
                }                
            } if (x_mine[0] == x_r[0] && y) { return true; }
            return false;
        }

        /// <summary>
        /// Перед использованием физики - запустить этот метод (но только один раз)
        /// </summary>
        internal void physic_on_start() { ph = new physic(get_coordinate()); }
        /// <summary>
        /// Перед использованием физики - запустить этот метод (но только один раз)
        /// </summary>
        /// <param name="mass_">Масса объекта</param>
        internal void physic_on_start(int mass_)
        { ph = new physic(get_coordinate(), mass_); }

        private physic ph;

        /// <summary>
        /// Падение (метод физики)
        /// </summary>
        /// <param name="arg_">Аргумент</param>
        /// <returns></returns>
        internal coordinate fall(int arg_)
        {            
            return ph.fall(arg_);
        }

        /// <summary>
        /// Устанавливаем объект по координатам
        /// </summary>
        /// <param name="c_">Координаты объекта</param>
        internal void set_location(coordinate c_)
        {
            pictureBox1.Location = new Point(c_.get_coordinate(0)[0],
                c_.get_coordinate(5)[0]);
        }

        /// <summary>
        /// Назначение новых координат
        /// </summary>
        /// <param name="c_"></param>
        internal void set_cordinate(coordinate c_)
        {
            coordinate_pb = c_;
        }

        /// <summary>
        /// Простое обновление координат pb-кса
        /// </summary>
        internal void update_coordinate()
        {
            coordinate my_coordinate = new coordinate(
                pictureBox1.Location.X,
                pictureBox1.Location.X,
                "left-x");//0
            my_coordinate.add(
                pictureBox1.Location.X,
                pictureBox1.Location.X + pictureBox1.Width,
                "top-x");//1
            my_coordinate.add(
                pictureBox1.Location.X + pictureBox1.Width,
                pictureBox1.Location.X + pictureBox1.Width,
                "right-x");//2
            my_coordinate.copy_and_add(1, "bottom-x");//3
            // realize copy
            my_coordinate.add(
                pictureBox1.Location.Y,
                pictureBox1.Location.Y + pictureBox1.Height,
                "left-y");//4
            my_coordinate.add(
                pictureBox1.Location.Y,
                pictureBox1.Location.Y,
                "top-y");//5
            my_coordinate.copy_and_add(4, "right-y");//6
            // realize copy
            my_coordinate.add(
                pictureBox1.Location.Y + pictureBox1.Height,
                pictureBox1.Location.Y + pictureBox1.Height,
                "bottom-y");//7
            coordinate_pb = my_coordinate;
        }

        /// <summary>
        /// Возвращение координат 
        /// </summary>
        /// <returns></returns>
        internal coordinate get_coordinate()
        { return coordinate_pb; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="works_">mine object</param>
        /// <param name="work_mine">other object</param>
        /// <returns></returns>
        internal bool eql_unsafe(List<work> works_, work work_mine)
        {
            //work work_ = work_mine;
            work_mine.update_coordinate();
            foreach (work item in works_)
            {
                item.update_coordinate();
                if (work_mine.get_coordinate().get_coordinate(1)[0] >=
                    item.get_coordinate().get_coordinate(1)[0]
                    && work_mine.get_coordinate().get_coordinate(1)[
                        work_mine.get_coordinate().get_coordinate(1).Length - 1] <=
                    item.get_coordinate().get_coordinate(1)[
                        item.get_coordinate().get_coordinate(1).Length - 1])
                {//проверка на иксы
                    if(work_mine.get_coordinate().get_coordinate(4)[0] >=
                        work_mine.get_coordinate().get_coordinate(4)[0] &&
                        work_mine.get_coordinate().get_coordinate(4)[
                            work_mine.get_coordinate().get_coordinate(4).Length - 1] <=
                            item.get_coordinate().get_coordinate(4)[
                                item.get_coordinate().get_coordinate(4).Length - 1])
                    {//проверка на игрики
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Возвращает имена пересекаемых объектов
        /// </summary>
        /// <param name="works_">mime object</param>
        /// <param name="work_mine">other object</param>
        /// <returns></returns>
        internal string[] eql_name_unsafe(List<work> works_, work work_mine)
        {
            //work work_ = work_mine;
            work_mine.update_coordinate();
            List<string> names_ = new List<string>();
            coordinate mine = work_mine.get_coordinate();
            foreach (work item in works_)
            {
                item.update_coordinate();
                bool l, r, t, b;
                //для права
                coordinate other = item.get_coordinate();
                //------------------------------------------
                if(mine.get_coordinate(4).Length < other.get_coordinate(4).Length
                    || true)
                    // ОТЛАДКА DEBUG убрать true
                {
                    if((mine.get_coordinate(4)[0] >= 
                        other.get_coordinate(4)[0] && 
                        mine.get_coordinate(4)[
                            mine.get_coordinate(4).Length - 1] <=
                            other.get_coordinate(4)[
                                other.get_coordinate(4).Length - 1])
                                || (mine.get_coordinate(4)[0] <= 
                                other.get_coordinate(4)[
                                    other.get_coordinate(4).Length - 1]
                                    && mine.get_coordinate(4)[
                                        mine.get_coordinate(4).Length - 1] >
                                    other.get_coordinate(4)[0]
                                       // other.get_coordinate(4).Length - 1] 
                                    )
                                 || (mine.get_coordinate(4)[
                                     mine.get_coordinate(4).Length - 1] >=
                                     other.get_coordinate(4)[0] 
                                     && mine.get_coordinate(4)[0] < 
                                     other.get_coordinate(4)[
                                         other.get_coordinate(4).Length - 1]
                                 )
                                 || (mine.get_coordinate(4)[0] <
                                 other.get_coordinate(4)[0] && 
                                 mine.get_coordinate(4)[
                                     mine.get_coordinate(4).Length - 1] >
                                     other.get_coordinate(4)[
                                         other.get_coordinate(4).Length - 1]))
                    {
                        // проверить
                        // проверка иксов
                        if (mine.get_coordinate(1)[0] <= 
                            other.get_coordinate(2)[
                                other.get_coordinate(2).Length - 1]
                                && mine.get_coordinate(2)[
                                    mine.get_coordinate(2).Length - 1] > 
                                    other.get_coordinate(2)[
                                        other.get_coordinate(2).Length - 1])
                        {
                            //string name = item.pictureBox1.Name;
                            names_.Add(item.pictureBox1.Name);
                        }
                        //if(mine.get_coordinate(1)[0])
                    }                    
                }
            }
            return names_.ToArray();
        }        
    }
    public class physic
    {
        private coordinate coordinate_ { get; set; }

        private static int arg_fall = 0;

        private int mass { get; set; }

        public physic(coordinate c_)
        {
            coordinate_ = c_;
            mass = 0;
        }

        public physic(coordinate c_, int mass_)
        {
            coordinate_ = c_;
            mass = mass_;
        }

        /// <summary>
        /// Ускорение св. падения (ТОЛЬКО ДЛЯ НЕНУЛЕВОЙ МАССЫ)
        /// </summary>
        /// <param name="arg">Коэфициент падения (на что прибаляем)</param>
        /// <returns></returns>
        internal coordinate fall(int arg)
        {
            //место для формулы
            arg_fall += arg;
            coordinate_.all_plus(coordinate_, 4, arg_fall);
            coordinate_.all_plus(coordinate_, 5, arg_fall);
            coordinate_.all_plus(coordinate_, 6, arg_fall);
            coordinate_.all_plus(coordinate_, 7, arg_fall);
            return coordinate_;
        }

        /// <summary>
        /// Ускорение св. падения (ТОЛЬКО ДЛЯ НЕНУЛЕВОЙ МАССЫ)
        /// </summary>
        /// <param name="arg">Коэфициент падения (на что прибаляем)</param>
        /// <returns></returns>
        internal coordinate fall_m(int arg)
        {
            //место для формулы
            arg_fall = (arg_fall*mass) + arg;
            coordinate_.all_plus(coordinate_, 4, arg_fall);
            coordinate_.all_plus(coordinate_, 5, arg_fall);
            coordinate_.all_plus(coordinate_, 6, arg_fall);
            coordinate_.all_plus(coordinate_, 7, arg_fall);
            return coordinate_;
        }
    }
}

namespace map
{
    //все остальные методы (кроме практических) должны быть созданы в ворке
    public class object_
    {
        private string name { get; set; }
        internal string get_name_() { return name; }
        //--------------------------------------------
        //--------------------------------------------
        private coordinate.work left { get; set; }
        private coordinate.work right { get; set; }
        private coordinate.work top { get; set; }
        private coordinate.work bottom { get; set; }
        private coordinate.work mine_obj { get; set; }
        //--------------------------------------------
        //public bool on_left { get; set; }// косаемся ли левого объекта
        //public bool on_right { get; set; }// эти переменные под вопросом
        //--------------------------------------------
        Func<int> fn_on_left { get; set; }
        Func<int> fn_on_right { get; set; }
        Func<int> fn_on_top { get; set; }
        Func<int> fn_on_bottom { get; set; }
        //---------------------------------------------------
        //---------------------------------------------------
        private int move_on_x { get; set; }
        private int move_on_y { get; set; }
        //---------------------------------------------------
        //---------------------------------------------------
        private List<coordinate.work> unsafe_works_;
        //---------------------------------------------------
        //---------------------------------------------------

        /// <summary>
        /// Включаем unsafe мод 
        /// </summary>
        internal void unsafe_mod()
        {
            unsafe_works_ = new List<coordinate.work>();
        }
        /// <summary>
        /// Добавляем рабочих для unsafe работы
        /// </summary>
        /// <param name="w_plus"></param>
        internal void unsafe_plus_work(coordinate.work w_plus)
        {
            unsafe_works_.Add(w_plus);
        }
        /// <summary>
        /// Пересекаемсяли с чем-нибудь???
        /// </summary>
        /// <returns></returns>
        internal bool unsafe_eql()
        {
            return mine_obj.eql_unsafe(unsafe_works_, mine_obj);
        }

        /// <summary>
        /// Имена пересечений
        /// </summary>
        /// <returns></returns>
        internal string[] unsafe_eql_string_array()
        {
            return mine_obj.eql_name_unsafe(unsafe_works_, mine_obj);
        }

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="t_">Верхний объект</param>
        /// <param name="b_">Нижний</param>
        /// <param name="l_">Левый</param>
        /// <param name="r_">Правый</param>
        /// <param name="m_">Наш (мы)</param>
        /// <param name="m_x">Скорость перемещения по Х</param>
        /// <param name="m_y">Скорость перемещения по У</param>
        public object_(string name_ ,coordinate.work t_, coordinate.work b_,
            coordinate.work l_, coordinate.work r_, coordinate.work m_,
            int m_x, int m_y)
        {
            name = name_;
            move_on_x = m_x;
            move_on_y = m_y;
            top = t_;
            bottom = b_;
            left = l_;
            right = r_;
            mine_obj = m_;
            fn_on_left = null_;
            fn_on_right = null_;
            fn_on_top = null_;
            fn_on_bottom = null_;
        }

        private int null_() { return 0; }

        /// <summary>
        /// Метод выполняющийся на пересечении объекта с объектом слева
        /// </summary>
        /// <param name="fn">Метод</param>
        internal void on_left(Func<int> fn)
        {
            fn_on_left = fn;
        }
        /// <summary>
        /// Метод выполняющийся на пересечении объекта с объектом справа
        /// </summary>
        /// <param name="fn">Метод</param>
        internal void on_right(Func<int> fn)
        {
            fn_on_right = fn;
        }
        /// <summary>
        /// Метод выполняющийся на пересечении объекта с объектом сверху
        /// </summary>
        /// <param name="fn">Метод</param>
        internal void on_top(Func<int> fn)
        {
            fn_on_top = fn;
        }
        /// <summary>
        /// Метод выполняющийся на пересечении объекта с объектом снизу
        /// </summary>
        /// <param name="fn">Метод</param>
        internal void on_bottom(Func<int> fn)
        {
            fn_on_bottom = fn;
        }

        /// <summary>
        /// Изменяем координату Х определённую при создании
        /// </summary>
        /// <param name="x">Положительно или отрицательно (true или false)</param>
        internal void plus_x(bool x)
        {
            coordinate.coordinate c_ = mine_obj.get_coordinate();
            if (x)
            {
                mine_obj.set_location(
                    c_.all_plus(
                        mine_obj.get_coordinate(), 0, move_on_x)
                ); mine_obj.update_coordinate();
                if (mine_obj.eql_right(right.get_coordinate())) { fn_on_right(); }
                // доделать
            }
            else
            {
                mine_obj.set_location(
                    c_.all_plus(
                        mine_obj.get_coordinate(), 0, -1*move_on_x)
                ); mine_obj.update_coordinate();
                if (mine_obj.eql_left(left.get_coordinate())) { fn_on_left(); }
                // доделать
            }
        }

        /// <summary>
        /// Прибавляем к координатам установленное значение У
        /// </summary>
        /// <param name="y">Вверхили вниз (true или false)</param>
        internal void plus_y(bool y)// true - вверх, false - вниз
        {
            coordinate.coordinate c_ = mine_obj.get_coordinate();
            if (y)
            {
                mine_obj.set_location(
                    c_.all_plus(
                        mine_obj.get_coordinate(), 5, -1 * move_on_y
                        )); mine_obj.update_coordinate();
                if (mine_obj.eql_top(top.get_coordinate())) { fn_on_top(); }
            }
            else
            {
                mine_obj.set_location(
                    c_.all_plus(
                        mine_obj.get_coordinate(), 7,  move_on_y
                        )); mine_obj.update_coordinate();
                if (mine_obj.eql_bottom(bottom.get_coordinate())) { fn_on_bottom(); }
            }
        }
    }
}