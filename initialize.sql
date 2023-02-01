CREATE DATABASE taekwondo_competition; 

CREATE TABLE taekwondo_competition.clubs (
	club_id   INT 		NOT NULL UNIQUE AUTO_INCREMENT,
    club_name CHAR(120) NOT NULL UNIQUE,
    city 	  CHAR(100) NOT NULL,
    gym_addr  CHAR(200) NOT NULL,
	PRIMARY KEY (club_id)
);

CREATE TABLE taekwondo_competition.sportsmans (
	membership_card_num INT NOT NULL UNIQUE AUTO_INCREMENT,
	sports_category 	CHAR(50),
    photo 				CHAR(50),
    coach_id 			INT,
    first_name			CHAR(60)   NOT NULL,
    last_name			CHAR(60)   NOT NULL,
    patronymic			CHAR(60)   NOT NULL,
    birth_date      	DATE       NOT NULL,
	password        	CHAR(100)  NOT NULL,
    belt            	ENUM(
		"1", "2,", "3", "4", "5", "6", "7", "8", "9", "10",
		"I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX")
        DEFAULT "10",
	sex             	ENUM("Ч", "Ж")           DEFAULT "Ч",
    role            	ENUM("Regular", "Admin") DEFAULT "Regular",
	PRIMARY KEY (membership_card_num)
);

CREATE TABLE taekwondo_competition.coaches (
	coach_id INT NOT NULL UNIQUE AUTO_INCREMENT,
    membership_card_num INT UNIQUE NOT NULL,
	instructor_category CHAR(50) NOT NULL,
    club_id INT NOT NULL,
    phone CHAR(16)  UNIQUE NOT NULL,
    email CHAR(254) UNIQUE,
    FOREIGN KEY (club_id) REFERENCES clubs(club_id)
    ON DELETE CASCADE
	ON UPDATE CASCADE,
    FOREIGN KEY (membership_card_num) REFERENCES sportsmans(membership_card_num)
    ON DELETE CASCADE
	ON UPDATE CASCADE,
    PRIMARY KEY (coach_id)
);

ALTER TABLE taekwondo_competition.sportsmans 
	  ADD FOREIGN KEY (coach_id) REFERENCES coaches(coach_id)
      ON DELETE SET NULL
      ON UPDATE CASCADE;

CREATE TABLE taekwondo_competition.judges (
	judge_id INT NOT NULL UNIQUE AUTO_INCREMENT,
    membership_card_num INT UNIQUE NOT NULL,
    judge_category CHAR(50) NOT NULL,
    FOREIGN KEY (membership_card_num) REFERENCES sportsmans(membership_card_num),
    PRIMARY KEY (judge_id)
);

CREATE TABLE taekwondo_competition.competitions (
    competition_id INT NOT NULL UNIQUE AUTO_INCREMENT,
    competition_name CHAR(100) NOT NULL,
    weighting_date DATE NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    city CHAR(30) NOT NULL,
    current_status ENUM(
		"очікується", 
        "прийом заявок", 
        "прийом заявок закінчено", 
        "в процесі", 
        "закінчено",
        "скасовано") DEFAULT "очікується",
    competition_level CHAR(50) DEFAULT "Інші турніри",
    CONSTRAINT unique_competition UNIQUE (competition_name, start_date),
    PRIMARY KEY (competition_id)
);

CREATE TABLE taekwondo_competition.divisions (
	division_name CHAR(50) UNIQUE NOT NULL,
    sex ENUM("Ч", "Ж") DEFAULT "Ч",
    min_weight SMALLINT,
    max_weight SMALLINT,
    min_age TINYINT NOT NULL,
    max_age TINYINT,
    min_belt ENUM(
		"1", "2,", "3", "4", "5", "6", "7", "8", "9", "10",
		"I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX")
        NOT NULL,
    max_belt ENUM(
		"1", "2,", "3", "4", "5", "6", "7", "8", "9", "10",
		"I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"),
    PRIMARY KEY (division_name)
); 

CREATE TABLE taekwondo_competition.competitors (
    application_num INT NOT NULL UNIQUE AUTO_INCREMENT,
    membership_card_num INT NOT NULL,
    competition_id INT NOT NULL,
    belt ENUM(
		"1", "2,", "3", "4", "5", "6", "7", "8", "9", "10",
		"I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX")
        NOT NULL,
	weighting_result SMALLINT,
    CONSTRAINT unique_competitor UNIQUE(membership_card_num, competition_id),
    FOREIGN KEY (membership_card_num) REFERENCES sportsmans(membership_card_num),
    FOREIGN KEY (competition_id) REFERENCES competitions(competition_id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    PRIMARY KEY (application_num)
);

CREATE TABLE taekwondo_competition.dayangs (
	dayang_id INT NOT NULL UNIQUE AUTO_INCREMENT,
    competition_id INT NOT NULL,
	management_access_key CHAR(60) NOT NULL UNIQUE,
	FOREIGN KEY (competition_id) REFERENCES competitions(competition_id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    PRIMARY KEY (dayang_id)
);

CREATE TABLE taekwondo_competition.judging_staff (
	application_num INT NOT NULL UNIQUE AUTO_INCREMENT,
    dayang_id INT NOT NULL,
    judge_id INT NOT NULL,
    judge_role ENUM(
		"боковий", 
        "рефері", 
        "головний", 
        "заступник головного судді")
        NOT NULL,
	CONSTRAINT unique_dayang_staff UNIQUE(dayang_id, judge_id),
    FOREIGN KEY (judge_id) REFERENCES judges(judge_id),
    FOREIGN KEY (dayang_id) REFERENCES dayangs(dayang_id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
	PRIMARY KEY (application_num)
);

CREATE TABLE taekwondo_competition.distributions (
	distribution_id INT 	 NOT NULL AUTO_INCREMENT,
    dayang_id 		INT 	 NOT NULL,
    division_name 	CHAR(50) NOT NULL,
    serial_num 		INT		 NOT NULL,
    CONSTRAINT unique_distribution UNIQUE(dayang_id, division_name),
    CONSTRAINT unique_serial_num   UNIQUE(dayang_id, serial_num),
    FOREIGN KEY (dayang_id) REFERENCES dayangs(dayang_id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    FOREIGN KEY (division_name) REFERENCES divisions(division_name),
	PRIMARY KEY (distribution_id)
);

CREATE TABLE taekwondo_competition.tossups (
	tossup_id INT NOT NULL UNIQUE AUTO_INCREMENT,
    competitor_in_blue INT NOT NULL,
    competitor_in_red  INT,
    lap_num 		   INT NOT NULL,
    pair_serial_num    INT NOT NULL,
    CONSTRAINT check_corner_competitors
		CHECK (competitor_in_blue != competitor_in_red),
	CONSTRAINT unique_competitor_in_blue
		UNIQUE (lap_num, competitor_in_blue),
	CONSTRAINT unique_competitor_in_red
		UNIQUE (lap_num, competitor_in_red),
    CONSTRAINT unique_pair 
		UNIQUE (competitor_in_blue, competitor_in_red),
    FOREIGN KEY (competitor_in_blue) 
		REFERENCES competitors(application_num),
	FOREIGN KEY (competitor_in_red) 
		REFERENCES competitors(application_num),
    PRIMARY KEY (tossup_id)
);

INSERT INTO taekwondo_competition.clubs(club_name, city, gym_addr) VALUES
	('СК "Білий Ведмідь"', 'Харків', 'проспект Космонавтів, 7, Вінниця, Вінницька область, 21000'),
    ('СК "Кобра-Кван"', 'Дніпро', 'проспект Космонавтів, 7, Вінниця, Вінницька область, 21000'),
    ('КДЮСШ "ТАЙФУН"', 'Київ', 'проспект Космонавтів, 7, Вінниця, Вінницька область, 21000'),
    ('СК "МАКСИМУМ"', 'Вінниця', 'проспект Космонавтів, 7, Вінниця, Вінницька область, 21000'),
    ('СК "Спарта"', 'Полтава', 'проспект Космонавтів, 7, Вінниця, Вінницька область, 21000');
    
INSERT INTO taekwondo_competition.sportsmans(membership_card_num, first_name, last_name, patronymic, sex, birth_date, belt, sports_category, password, role) VALUES
    (2, "Половинко", "Йосип", "Леонідович", "Ч", "1979-09-17", "VII", "Майстер спорту", "admin", "Admin"),
    (5,	"Бартків",	 "Белла", "Тарасівна",  "Ж", "1978-09-17", "VII", "Майстер спорту", "admin2", "Admin");

INSERT INTO taekwondo_competition.sportsmans(membership_card_num, first_name, last_name, patronymic, sex, birth_date, belt, sports_category, password, role) VALUES
    (1,  "Стягайло",   "Царук",      "Янович",     "Ч", "1985-09-17",	"VI",	"Майстер спорту", "coach1", "Regular"),
    (32, "Гошовський", "Євгеній",    "Макарович",  "Ч", "1986-09-17",	"V",	"Майстер спорту", "coach2", "Regular"),
    (56, "Мичко",      "Дорогобуг",  "Максимович", "Ч", "1983-09-17",	"VI",	"Майстер спорту", "coach3", "Regular"),
    (78, "Мошак",      "Цвітан",     "Леонідович", "Ч", "1981-09-17",	"V",	"Майстер спорту", "coach4", "Regular"),
    (91, "Погорелов",  "Жито",       "Омелянович", "Ч", "1985-09-17",	"VI",	"Майстер спорту", "coach5", "Regular"),
    (87, "Теличенко",  "Килина",     "Янівна",     "Ж", "1986-09-17",	"V",	"Майстер спорту", "coach6", "Regular"),
    (13, "Гарасимів",  "Севастіана", "Юліанівна",  "Ж", "1983-09-17",	"VI",	"Майстер спорту", "coach7", "Regular");

INSERT INTO taekwondo_competition.coaches(membership_card_num, instructor_category, club_id, phone, email) VALUES
    (1,  "Інструктор міжнародного класу", 1,  "+380666238821", "bosubum1@ukr.net"),
    (87, "Інструктор міжнародного класу", 2,  "+380666238822", "bosubum2@ukr.net"),
    (32, "Тренер національного класу",    3,  "+380666238823", "bosubum3@ukr.net"),
    (56, "Тренер національного класу",    4, "+380666238824", "bosubum4@ukr.net"),
    (13, "Тренер національного класу",    5, "+380666238825", "bosubum5@ukr.net"),
    (78, "Тренер початкової підготовки",  1,  "+380666238826", "bosubum6@ukr.net"),
    (91, "Тренер початкової підготовки",  2,  "+380666238827", "bosubum7@ukr.net");
    
INSERT INTO taekwondo_competition.sportsmans(membership_card_num, first_name, last_name, patronymic, sex, birth_date, belt, coach_id, password, role) VALUES
    (3084,  "Гречко",         "Рід",         "Артемович",     "Ч",	"2002-11-21",	"III", 1,  "1", "Regular"),                      
    (3134,  "Зазуляк",        "Тур",         "Давидович",     "Ч",	"1996-07-14",	"III", 2,  "2", "Regular"),                      
    (3125,  "Якимчук",        "Жадан",       "Фролович",      "Ч",	"1999-01-16",	"II",  3,  "3", "Regular"),                  
    (5240,  "Сокур",          "Тімох",       "Максимович",    "Ч",	"2004-12-17",	"II",  4,  "4", "Regular"),                      
    (3122,  "Лобай",          "Честислав",   "Бажанович",     "Ч",	"1999-01-05",	"II",  5,  "5", "Regular"),                          
    (8046,  "Бовкун",         "Юліан",       "Семенович",     "Ч",	"2006-06-16",	"I",   1,  "6", "Regular"),                    
    (3083,  "Засядько",       "Перемисл",    "Давидович",     "Ч",	"2002-07-29",	"I",   2,  "7", "Regular"),                        
    (14675, "Головатий",      "Євстафій",    "Фролович",      "Ч",	"2007-08-08",	"I",   3,  "8", "Regular"),                     
    (8097,  "Поплавський",    "Пребислав",   "Чеславович",    "Ч",	"2005-07-06",	"I",   4,  "9", "Regular"),                         
    (5246,  "Німченко",       "Веселан",     "Олегович",      "Ч",	"2004-06-28",	"I",   5,  "10", "Regular"),                      
    (5166,  "Липа",           "Ходота",      "Семенович",     "Ч",	"2009-10-27",	"I",   1,  "11", "Regular"),                     
    (3091,  "Пероганич",      "Панас",       "Остапович",     "Ч",	"2003-01-18",	"I",   2,  "12", "Regular"),                     
    (18452, "Самсоненко",     "Колодій",     "Пилипович",     "Ч",	"2000-05-30",	"I",   3,  "13", "Regular"),                          
    (7771,  "Зубицький",      "Святослав",   "Богуславович",  "Ч",	"2008-11-17",	"I",   4,  "14", "Regular"),                          
    (4669,  "Кобець",         "Чернин",      "Добромирович",  "Ч",	"2002-11-18",	"I",   5,  "15", "Regular"),                      
    (8096,  "Бурмака",        "Потап",       "Ярославович",   "Ч",	"2008-01-07",	"I",   1,  "16", "Regular"),                     
    (6716,  "Медуниця",       "Честислав",   "Пилипович",     "Ч",	"2005-06-19",	"I",   2,  "17", "Regular"),                         
    (8039,  "Хмельницький",   "Шарль",       "Азарович",      "Ч",	"2004-08-16",	"I",   3,  "18", "Regular"),                  
    (4811,  "Наливайко",      "Атанас",      "Вікторович",    "Ч",	"2007-09-03",	"I",   4,  "19", "Regular"),                      
    (3080,  "Миколюк",        "Щастибог",    "Орестович",     "Ч",	"2004-05-12",	"I",   5,  "20", "Regular"),                          
    (2785,  "Сільченко",      "Любим",       "Тихонович",     "Ч",	"2001-05-22",	"I",   1,  "21", "Regular"),                     
    (6759,  "Шкурган",        "Йоханес",     "Драганович",    "Ч",	"2004-01-17",	"I",   2,  "22", "Regular"),                         
    (5237,  "Федина",         "Єгор",        "Сарматович",    "Ч",	"2007-03-05",	"I",   3,  "23", "Regular"),                      
    (2789,  "Кондратюк",      "Щек",         "Семенович",     "Ч",	"2002-01-24",	"I",   4,  "24", "Regular"),                      
    (5238,  "Колосовський",   "Силослав",    "Добромирович",  "Ч",	"2005-03-26",	"I",   5,  "25", "Regular"),                          
    (7862,  "Пісецький",      "Євлампій",    "Давидович",     "Ч",	"1991-10-03",	"I",   1,  "26", "Regular"),                         
    (6531,  "Римаренко",      "Йосип",       "Герасимович",   "Ч",	"2004-07-08",	"I",   2,  "27", "Regular"),                     
    (5186,  "Комар",          "Чесмил",      "Олегович",      "Ч",	"2007-05-27",	"I",   3,  "28", "Regular"),                  
    (5241,  "Баштан",         "Сонцелик",    "Орестович",     "Ч",	"2005-02-20",	"I",   4,  "29", "Regular"),                          
    (5236,  "Тимчук",         "Чеслав",      "Владиславович", "Ч",	"2006-10-07",	"I",   5,  "30", "Regular"),                          
    (5050,  "Білоусенко",     "Ус",          "Владиславович", "Ч",	"2004-06-06",	"I",   1,  "31", "Regular"),                     
    (8092,  "Савко",          "Щастислав",   "Русланович",    "Ч",	"2007-09-26",	"1",   2,  "32", "Regular"),                         
    (6712,  "Юровський",      "Амвросій",    "Устимович",     "Ч",	"2008-04-11",	"1",   3,  "33", "Regular"),                          
    (7959,  "Белькевич",      "Віст",        "Охримович",     "Ч",	"2007-08-17",	"1",   4,  "34", "Regular"),                      
    (15333, "Радиш",          "Йосеф",       "Денисович",     "Ч",	"2010-11-20",	"1",   5,  "35", "Regular"),                      
    (16631, "Вірастюк",       "Порфир",      "Валентинович",  "Ч",	"2007-12-26",	"2",   1,  "36", "Regular"),                     
    (8098,  "Мазурок",        "Зборислав",   "Охримович",     "Ч",	"2007-08-01",	"2",   2,  "37", "Regular"),                         
    (6713,  "Крук",           "Федір",       "Олександрович", "Ч",	"2007-12-06",	"2",   3,  "38", "Regular"),                          
    (12064, "Заброда",        "Іоанн",       "Охримович",     "Ч",	"2010-05-07",	"2",   4,  "39", "Regular"),                      
    (7751,  "Юхименко",       "Собіслав",    "Омелянович",    "Ч",	"2010-01-03",	"2",   5,  "40", "Regular"),                          
    (8090,  "Зарицький",      "Йоган",       "Денисович",     "Ч",	"2009-06-30",	"2",   1,  "41", "Regular"),                     
    (6767,  "Солдатенко",     "Велемир",     "Тарасович",     "Ч",	"2008-07-30",	"2",   2,  "42", "Regular"),                         
    (6779,  "Тищик",          "Борислав",    "Охримович",     "Ч",	"2009-07-22",	"2",   3,  "43", "Regular"),                          
    (11896, "Майданюк",       "Царук",       "Соломонович",   "Ч",	"2009-12-02",	"3",   4,  "44", "Regular"),                      
    (8122,  "Нагорняк",       "Щастислав",   "Панасович",     "Ч",	"2008-10-14",	"3",   5,  "45", "Regular"),                          
    (8099,  "Трохименко",     "Щедрогост",   "Златович",      "Ч",	"2007-06-20",	"3",   1,  "46", "Regular"),                     
    (8124,  "Біда",           "Шарль",       "Панасович",     "Ч",	"2007-11-25",	"3",   2,  "47", "Regular"),                     
    (6778,  "Безкровний",     "Олесь",       "Давидович",     "Ч",	"2009-03-02",	"3",   3,  "48", "Regular"),                      
    (9772,  "Ґут",            "Кульчицький", "Фролович",      "Ч",	"2010-01-25",	"3",   4,  "49", "Regular"),                           
    (8125,  "Бурмака",        "Остромисл",   "Іванович",      "Ч",	"2008-04-30",	"3",   5,  "50", "Regular"),                      
    (7161,  "Марусик",        "Олександр",   "Леонідович",    "Ч",	"2010-10-21",	"3",   1,  "51", "Regular"),                         
    (11899, "Незабитовський", "Йоханес",     "Іванович",      "Ч",	"2009-08-11",	"3",   2,  "52", "Regular"),                     
    (11894, "Гасай",          "Воля",        "Адамович",      "Ч",	"2008-12-27",	"3",   3,  "53", "Regular"),                  
    (17079, "Караванський",   "Віктор",      "Гордиславович", "Ч",	"2006-04-07",	"3",   4,  "54", "Regular"),                           
    (18458, "Воробей",        "Флор",        "Яромирович",    "Ч",	"2011-06-16",	"4",   5,  "55", "Regular"),                      
    (19544, "Заставний",      "Еразм",       "Юліанович",     "Ч",	"2010-12-21",	"5",   6,  "56", "Regular"),                      
    (18447, "Самборський",    "Світолюб",    "Володимирович", "Ч",	"2011-07-07",	"5",   7,  "57", "Regular"),                              
    (9737,  "Кабаченко",      "Ізяслав",     "Августинович",  "Ч",	"2009-03-05",	"5",   6,  "58", "Regular"),                          
    (18450, "Топіга",         "Гостомисл",   "Олександрович", "Ч",	"2011-08-22",	"5",   7,  "59", "Regular"),                              
    (19543, "Бутовичі",       "Фрол",        "Устимович",     "Ч",	"2010-04-03",	"5",   6,  "60", "Regular"),                      
    (14680, "Греба",          "Чара",        "Вікторович",    "Ч",	"2010-02-03",	"5",   7,  "61", "Regular"),                      
    (18449, "Томашевський",   "Честислав",   "Федорович",     "Ч",	"2011-02-16",	"5",   6,  "62", "Regular"),                          
    (19542, "Проненко",       "Роман",       "Давидович",     "Ч",	"2011-06-20",	"5",   7,  "63", "Regular"),                      
    (11920, "Храневич",       "Юхим",        "Адріанович",    "Ч",	"2009-07-01",	"5",   6,  "64", "Regular"),                      
    (31469, "Гречко",         "Андрій",      "Вітанович",     "Ч",	"2008-07-21",	"5",   7,  "65", "Regular"),                      
    (18448, "Гойчук",         "Зорегляд",    "Орестович",     "Ч",	"2010-11-22",	"5",   4,  "66", "Regular");

INSERT INTO taekwondo_competition.sportsmans(membership_card_num, first_name, last_name, patronymic, sex, birth_date, belt, coach_id, password, role) VALUES
    (3074,	"Неділько",    "Йосипа",      "Северинівна",    "Ж", "2004-03-23",	"III", 1,  "68", "Regular"),                  
    (5245,	"Візерські",   "Юлина",       "Тарасівна",      "Ж", "2005-11-25",	"II",  2,  "69", "Regular"),                
    (5244,	"Шишацька",    "Елеонора",    "Полянівна",      "Ж", "2005-09-23",	"II",  3,  "70", "Regular"),                
    (3093,	"Діброва",     "Броніслава",  "Адріанівна",     "Ж", "2002-03-16",	"I",   4,  "71", "Regular"),                
    (8120,	"Чебикін",     "Інна",        "Макарівна",      "Ж", "2007-02-27",	"I",   5,  "72", "Regular"),               
    (22170,	"Салко",       "Позвізда",    "Ратиборівна",    "Ж", "2004-04-29",	"I",   1,  "73", "Regular"),                 
    (8095,	"Андрунців",   "Гликерія",    "Устимівна",      "Ж", "2008-01-07",	"I",   2,  "74", "Regular"),               
    (11873,	"Зеров",       "Пава",        "Федорівна",      "Ж", "2007-05-10",	"I",   3,  "75", "Regular"),               
    (6760,	"Драч",        "Шанетта",     "Олександрівна",  "Ж", "2007-06-12",	"I",   4,  "76", "Regular"),                   
    (3095,	"Вус",         "Богдана",     "Герасимівна",    "Ж", "2000-08-14",	"I",   5,  "77", "Regular"),                 
    (11893,	"Верес",       "Зореслава",   "Милославівна",   "Ж", "2007-05-03",	"I",   1,  "78", "Regular"),                  
    (6774,	"Шкурган",     "Йовілла",     "Іванівна",       "Ж", "2007-05-13",	"I",   2,  "79", "Regular"),              
    (8121,	"Остапенко",   "Чеслава",     "Богданівна",     "Ж", "2006-06-09",	"I",   3,  "80", "Regular"),                
    (8926,	"Васьків",     "Марія",       "Никодимівна",    "Ж", "1999-08-07",	"I",   4,  "81", "Regular"),                 
    (8082,	"Голубничий",  "Югина",       "Арсенівна",      "Ж", "2008-09-06",	"2",   5,  "82", "Regular"),               
    (6718,	"Самборська",  "Наталія",     "Найденівна",     "Ж", "2009-03-13",	"2",   1,  "83", "Regular"),                
    (6694,	"Ханенки",     "Ждана",       "Юхимівна",       "Ж", "2010-01-13",	"2",   2,  "84", "Regular"),              
    (22718,	"Зайчук",      "Шушана",      "Русланівна",     "Ж", "2011-02-15",	"2",   3,  "85", "Regular"),                
    (11898,	"Лісничий",    "Святослава",  "Полянівна",      "Ж", "2010-08-14",	"3",   4,  "86", "Regular"),               
    (11919,	"Фіалкович",   "Зореслава",   "Орестівна",      "Ж", "2010-09-13",	"5",   5,  "87", "Regular"),               
    (14676,	"Лавренко",    "Дзвенислава", "Любомирівна",    "Ж", "2008-12-12",	"5",   6,  "89", "Regular"),                 
    (30339,	"Кротюк",      "Дарія",       "Ростиславівна",  "Ж", "2010-09-30",	"5",   7,  "90", "Regular"),                    
    (11901,	"Михалюк",     "Євгенія",     "Устимівна",      "Ж", "2009-03-27",	"5",   6,  "91", "Regular");   
    
INSERT INTO taekwondo_competition.judges(membership_card_num, judge_category) VALUES
    (2785,	"Суддя 2-ї категорії"),
    (3074,	"Суддя 2-ї категорії"),
    (3084,	"Суддя 2-ї категорії"),
    (3091,	"Суддя 2-ї категорії"),
    (3093,	"Суддя 2-ї категорії"),
    (3095,	"Суддя 2-ї категорії"),
    (5244,	"Суддя 2-ї категорії"),
    (5245,	"Суддя 2-ї категорії"),
    (5246,	"Суддя 2-ї категорії"),
    (5050,	"Суддя 2-ї категорії"),
    (6531,	"Суддя 2-ї категорії"),
    (6760,	"Суддя 2-ї категорії"),
    (32,	"Суддя міжнародної категорії"),
    (3134,	"Суддя національної категорії");

INSERT INTO taekwondo_competition.divisions(division_name, sex, min_age, max_age, min_weight, max_weight, min_belt, max_belt) VALUES
    ("Хлопці 11-13 років -35кг",         "Ч", 11, 13, null, 35,	  3, null),
    ("Дівчата 11-13 років -35кг",        "Ж", 11, 13, 30,	35,   3, null),
    ("Дівчата 11-13 років -40кг",        "Ж", 11, 13, 35,	40,   3, null),
    ("Хлопці 11-13 років -40кг",         "Ч", 11, 13, 35,	40,   3, null),
    ("Дівчата 11-13 років -45кг",        "Ж", 11, 13, 40,	45,   3, null),
    ("Хлопці 11-13 років -45кг",         "Ч", 11, 13, 40,	45,   3, null),
    ("Дівчата 11-13 років -50кг",        "Ж", 11, 13, 45,	50,   3, null),
    ("Хлопці 11-13 років -50кг",         "Ч", 11, 13, 45,	50,   3, null),
    ("Дівчата 11-13 років -55кг",        "Ж", 11, 13, 50,	55,   3, null),
    ("Хлопці 11-13 років -55кг",         "Ч", 11, 13, 50,	55,   3, null),
    ("Дівчата 11-13 років 55+кг",        "Ж", 11, 13, 55,	null, 3, null),
    ("Хлопці 11-13 років -60кг",         "Ч", 11, 13, 55,	60,   3, null),
    ("Хлопці 11-13 років 60+кг",         "Ч", 11, 13, 60,	null, 3, null),
    ("Юніорки 14-15 років -40кг IДив",   "Ж", 14, 15, null, 40,	  5, 3   ),
    ("Юніори 14-15 років -45кг IДив",    "Ч", 14, 15, null, 45,	  5, 3   ),
    ("Юніорки 14-15 років -40кг IIДив",  "Ж", 14, 15, null, 40,	  2, null),
    ("Юніори 14-15 років -45кг IIДив",   "Ч", 14, 15, null, 45,	  2, null),
    ("Юніорки 14-15 років -45кг IДив",   "Ж", 14, 15, 40,	45,   5, 3   ),
    ("Юніорки 14-15 років -45кг IIДив",  "Ж", 14, 15, 40,	45,   2, null),
    ("Юніорки 14-15 років -50кг IДив",   "Ж", 14, 15, 45,	50,   5, 3   ),
    ("Юніори 14-15 років -50кг IДив",    "Ч", 14, 15, 45,	50,   5, 3   ),
    ("Юніорки 14-15 років -50кг IIДив",  "Ж", 14, 15, 45,	50,   2, null),
    ("Юніори 14-15 років -50кг IIДив",   "Ч", 14, 15, 45,	50,   2, null),
    ("Юніорки 14-15 років -55кг IДив",   "Ж", 14, 15, 50,	55,   5, 3   ),
    ("Юніори 14-15 років -55кг IДив",    "Ч", 14, 15, 50,	55,   5, 3   ),
    ("Юніорки 14-15 років -55кг IIДив",  "Ж", 14, 15, 50,	55,   2, null),
    ("Юніори 14-15 років -55кг IIДив",   "Ч", 14, 15, 50,	55,   2, null),
    ("Юніорки 14-15 років -60кг IДив",   "Ж", 14, 15, 55,	60,   5, 3   ),
    ("Юніори 14-15 років -60кг IДив",    "Ч", 14, 15, 55,	60,   5, 3   ),
    ("Юніорки 14-15 років -60кг IIДив",  "Ж", 14, 15, 55,	60,   2, null),
    ("Юніори 14-15 років -60кг IIДив",   "Ч", 14, 15, 55,	60,   2, null),
    ("Юніорки 14-15 років -65кг IДив",   "Ж", 14, 15, 60,	65,   5, 3   ),
    ("Юніори 14-15 років -65кг IДив",    "Ч", 14, 15, 60,	65,   5, 3   ),
    ("Юніорки 14-15 років -65кг IIДив",  "Ж", 14, 15, 60,	65,   2, null),
    ("Юніори 14-15 років -65кг IIДив",   "Ч", 14, 15, 60,	65,   2, null),
    ("Юніорки 14-15 років 65+кг IДив",   "Ж", 14, 15, 65,	null, 5, 3   ),
    ("Юніори 14-15 років -70кг IДив",    "Ч", 14, 15, 65,	70,   5, 3   ),
    ("Юніорки 14-15 років 65+кг IIДив",  "Ж", 14, 15, 65,	null, 2, null),
    ("Юніори 14-15 років -70кг IIДив",   "Ч", 14, 15, 65,	70,   2, null),
    ("Юніори 14-15 років 70+кг IДив",    "Ч", 14, 15, 70,	null, 5, 3   ),
    ("Юніори 14-15 років 70+кг IIДив",   "Ч", 14, 15, 70,	null, 2, null),
    ("Юніорки 16-17 років -40кг IДив",   "Ж", 16, 17, null, 40,	  5, 3   ),
    ("Юніори 16-17 років -45кг IДив",    "Ч", 16, 17, null, 45,	  5, 3   ),
    ("Юніорки 16-17 років -40кг IIДив",  "Ж", 16, 17, null, 40,	  2, null),
    ("Юніори 16-17 років -45кг IIДив",   "Ч", 16, 17, null, 45,	  2, null),
    ("Юніорки 16-17 років -46кг IДив",   "Ж", 16, 17, 40,	46,   5, 3   ),
    ("Юніорки 16-17 років -46кг IIДив",  "Ж", 16, 17, 40,	46,   2, null),
    ("Юніори 16-17 років -51кг IДив",    "Ч", 16, 17, 45,	51,   5, 3   ),
    ("Юніори 16-17 років -51кг IIДив",   "Ч", 16, 17, 45,	51,   2, null),
    ("Юніорки 16-17 років -52кг IДив",   "Ж", 16, 17, 46,	52,   5, 3   ),
    ("Юніорки 16-17 років -52кг IIДив",  "Ж", 16, 17, 46,	52,   2, null),
    ("Юніори 16-17 років -57кг IДив",    "Ч", 16, 17, 51,	57,   5, 3   ),
    ("Юніори 16-17 років -57кг IIДив",   "Ч", 16, 17, 51,	57,   2, null),
    ("Юніорки 16-17 років -58кг IДив",   "Ж", 16, 17, 52,	58,   5, 3   ),
    ("Юніорки 16-17 років -58кг IIДив",  "Ж", 16, 17, 52,	58,   2, null),
    ("Юніори 16-17 років -63кг IДив",    "Ч", 16, 17, 57,	63,   5, 3   ),
    ("Юніори 16-17 років -63кг IIДив",   "Ч", 16, 17, 57,	63,   2, null),
    ("Юніорки 16-17 років -64кг IДив",   "Ж", 16, 17, 58,	64,   5, 3   ),
    ("Юніорки 16-17 років -64кг IIДив",  "Ж", 16, 17, 58,	64,   2, null),
    ("Юніори 16-17 років -69кг IДив",    "Ч", 16, 17, 63,	69,   5, 3   ),
    ("Юніори 16-17 років -69кг IIДив",   "Ч", 16, 17, 63,	69,   2, null),
    ("Юніорки 16-17 років -70кг IДив",   "Ж", 16, 17, 64,	70,   5, 3   ),
    ("Юніорки 16-17 років -70кг IIДив",  "Ж", 16, 17, 64,	70,   2, null),
    ("Юніори 16-17 років -75кг IДив",    "Ч", 16, 17, 69,	75,   5, 3   ),
    ("Юніори 16-17 років -75кг IIДив",   "Ч", 16, 17, 69,	75,   2, null),
    ("Юніорки 16-17 років 70+кг IДив",   "Ж", 16, 17, 70,	null, 5, 3   ),
    ("Юніорки 16-17 років 70+кг IIДив",  "Ж", 16, 17, 70,	null, 2, null),
    ("Юніори 16-17 років 75+кг IДив",    "Ч", 16, 17, 75,	null, 5, 3   ),
    ("Юніори 16-17 років 75+кг IIДив",   "Ч", 16, 17, 75,	null, 2, null),
    ("Жінки 18-39 років -47кг IДив",     "Ж", 18, 39, null, 47,	  5, 3   ),
    ("Чоловіки 18-39 років -52кг IДив",  "Ч", 18, 39, null, 52,	  5, 3   ),
    ("Жінки 18-39 років -47кг IIДив",    "Ж", 18, 39, null, 47,	  2, null),
    ("Чоловіки 18-39 років -52кг IIДив", "Ч", 18, 39, null, 52,	  2, null),
    ("Жінки 18-39 років -52кг IДив",     "Ж", 18, 39, 47,	52,   5, 3   ),
    ("Жінки 18-39 років -52кг IIДив",    "Ж", 18, 39, 47,	52,   2, null),
    ("Жінки 18-39 років -57кг IДив",     "Ж", 18, 39, 52,	57,   5, 3   ),
    ("Чоловіки 18-39 років -58кг IДив",  "Ч", 18, 39, 52,	58,   5, 3   ),
    ("Жінки 18-39 років -57кг IIДив",    "Ж", 18, 39, 52,	57,   2, null),
    ("Чоловіки 18-39 років -58кг IIДив", "Ч", 18, 39, 52,	58,   2, null),
    ("Жінки 18-39 років -62кг IДив",     "Ж", 18, 39, 57,	62,   5, 3   ),
    ("Жінки 18-39 років -62кг IIДив",    "Ж", 18, 39, 57,	62,   2, null),
    ("Чоловіки 18-39 років -64кг IДив",  "Ч", 18, 39, 58,	64,   5, 3   ),
    ("Чоловіки 18-39 років -64кг IIДив", "Ч", 18, 39, 58,	64,   2, null),
    ("Жінки 18-39 років -67кг IДив",     "Ж", 18, 39, 62,	67,   5, 3   ),
    ("Жінки 18-39 років -67кг IIДив",    "Ж", 18, 39, 62,	67,   2, null),
    ("Чоловіки 18-39 років -71кг IДив",  "Ч", 18, 39, 64,	71,   5, 3   ),
    ("Чоловіки 18-39 років -71кг IIДив", "Ч", 18, 39, 64,	71,   2, null),
    ("Жінки 18-39 років -72кг IДив",     "Ж", 18, 39, 67,	72,   5, 3   ),
    ("Жінки 18-39 років -72кг IIДив",    "Ж", 18, 39, 67,	72,   2, null),
    ("Чоловіки 18-39 років -78кг IДив",  "Ч", 18, 39, 71,	78,   5, 3   ),
    ("Чоловіки 18-39 років -78кг IIДив", "Ч", 18, 39, 71,	78,   2, null),
    ("Жінки 18-39 років -77кг IДив",     "Ж", 18, 39, 72,	77,   5, 3   ),
    ("Жінки 18-39 років -77кг IIДив",    "Ж", 18, 39, 72,	77,   2, null),
    ("Жінки 18-39 років 77+кг IДив",     "Ж", 18, 39, 77,	null, 5, 3   ),
    ("Жінки 18-39 років 77+кг IIДив",    "Ж", 18, 39, 77,	null, 2, null),
    ("Чоловіки 18-39 років -85кг IДив",  "Ч", 18, 39, 78,	85,   5, 3   ),
    ("Чоловіки 18-39 років -85кг IIДив", "Ч", 18, 39, 78,	85,   2, null),
    ("Чоловіки 18-39 років -92кг IДив",  "Ч", 18, 39, 85,	92,   5, 3   ),
    ("Чоловіки 18-39 років -92кг IIДив", "Ч", 18, 39, 85,	92,   2, null),
    ("Чоловіки 18-39 років 92+кг IДив",  "Ч", 18, 39, 92,	null, 5, 3   ),
    ("Чоловіки 18-39 років 92+кг IIДив", "Ч", 18, 39, 92,	null, 2, null);
    
INSERT INTO taekwondo_competition.competitions(competition_name, weighting_date, start_date, end_date, city, current_status, competition_level) VALUES 
	("Кубок України серед старших юнаків, юніорів, дорослих",     "2022-02-03", "2022-02-04", "2022-02-06", "Черкаси",	 "очікується",    "Кубок України"),
    ("Чемпіонат України серед старших юнаків, юніорів, дорослих", "2023-02-16", "2023-02-16", "2023-02-19", "Вінниця",	 "прийом заявок", "Чемпіонат України"),	 	 
    ("Чемпіонат Вінницької області",	                          "2023-02-03", "2023-02-03", "2023-02-05",	"Вінниця",	 "прийом заявок", "Чемпіонат/кубок області"),	 
    ("Чемпіонат Дніпропетровській області",	                      "2023-01-28", "2023-01-28", "2023-01-28", "Дніпро",	 "закінчено",     "Чемпіонат/кубок області"),	 	 
    ("Відкритий Кубок Полтавської області",                       "2022-12-10", "2022-12-10", "2022-12-10", "Полтава",	 "закінчено",     "Чемпіонат/кубок області"),	 	 
    ("Відкритий кубок міста Вінниці",                             "2022-12-09", "2022-12-09", "2022-12-10",	"Вінниця",	 "закінчено",     "Інші всеукраїнські турніри"),
    ("Кубок Любарта 2022",                                        "2022-12-04", "2022-12-04", "2022-12-04", "Луцьк",	 "закінчено",     "Чемпіонат/кубок області"),
    ('Відкритий турнір СК "Прайд"',                               "2022-04-23", "2022-04-23", "2022-04-23",	"Запоріжжя", "скасовано",     "Інші турніри");