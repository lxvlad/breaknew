PRAGMA foreign_keys = ON;

-- =====================
--   TABLE: Users
-- =====================
CREATE TABLE IF NOT EXISTS Users (
  user_id     INTEGER PRIMARY KEY AUTOINCREMENT,
  user_name   TEXT NOT NULL,
  email       TEXT UNIQUE,
  password    TEXT NOT NULL,
  created_at  DATE NOT NULL
);

-- =====================
--   TABLE: Habits
-- =====================
CREATE TABLE IF NOT EXISTS Habits (
  habit_id       INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id        INTEGER NOT NULL,
  habit_name     TEXT NOT NULL,
  start_date     DATE NOT NULL,
  goal_days      INTEGER NOT NULL,
  motivation     TEXT,
  is_active      INTEGER NOT NULL CHECK(is_active IN (0,1)),
  total_days     INTEGER NOT NULL DEFAULT 0,
  current_streak INTEGER NOT NULL DEFAULT 0,
  total_saving   NUMERIC NOT NULL DEFAULT 0,
  FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS IX_Habits_User ON Habits(user_id);

-- =====================
--   TABLE: DailyStatuses
-- =====================
CREATE TABLE IF NOT EXISTS DailyStatuses (
  status_id      INTEGER PRIMARY KEY AUTOINCREMENT,
  habit_id       INTEGER NOT NULL,
  date           DATE NOT NULL,
  is_clean       INTEGER NOT NULL CHECK(is_clean IN (0,1)),
  trigger        TEXT NOT NULL,
  note           TEXT,
  craving_level  INTEGER NOT NULL CHECK(craving_level BETWEEN 0 AND 10),
  FOREIGN KEY (habit_id) REFERENCES Habits(habit_id) ON DELETE CASCADE
);
CREATE UNIQUE INDEX IF NOT EXISTS UX_DailyStatuses_habit_date ON DailyStatuses(habit_id, date);

-- =====================
--   TABLE: Savings
-- =====================
CREATE TABLE IF NOT EXISTS Savings (
  saving_id INTEGER PRIMARY KEY AUTOINCREMENT,
  habit_id  INTEGER NOT NULL,
  amount    NUMERIC NOT NULL CHECK(amount >= 0),
  FOREIGN KEY (habit_id) REFERENCES Habits(habit_id) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS IX_Savings_Habit ON Savings(habit_id);

-- =====================
--   TABLE: Achievements
-- =====================
CREATE TABLE IF NOT EXISTS Achievements (
  achievement_id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id        INTEGER NOT NULL,
  title          TEXT NOT NULL,
  achieved_at    DATE NOT NULL,
  FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS IX_Achievements_User ON Achievements(user_id);

-- =====================
--   TABLE: SOSActions
-- =====================
CREATE TABLE IF NOT EXISTS SOSActions (
  action_id INTEGER PRIMARY KEY AUTOINCREMENT,
  text      TEXT NOT NULL,
  category  TEXT NOT NULL
);

-- =====================
--   TABLE: UserSOSLogs
-- =====================
CREATE TABLE IF NOT EXISTS UserSOSLogs (
  log_id    INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id   INTEGER NOT NULL,
  action_id INTEGER NOT NULL,
  date      DATE NOT NULL,
  worked    INTEGER NOT NULL CHECK(worked IN (0,1)),
  FOREIGN KEY (user_id)   REFERENCES Users(user_id)    ON DELETE CASCADE,
  FOREIGN KEY (action_id) REFERENCES SOSActions(action_id) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS IX_UserSOSLogs_User ON UserSOSLogs(user_id);
CREATE INDEX IF NOT EXISTS IX_UserSOSLogs_Action ON UserSOSLogs(action_id);

-- =====================
--   TABLE: Quotes
-- =====================
CREATE TABLE IF NOT EXISTS Quotes (
  quote_id INTEGER PRIMARY KEY AUTOINCREMENT,
  text     TEXT NOT NULL
);
