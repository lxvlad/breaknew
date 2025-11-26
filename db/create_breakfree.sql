PRAGMA foreign_keys = ON;

-- =====================
--   TABLE: Users
-- =====================
CREATE TABLE IF NOT EXISTS Users (
    user_id     INTEGER PRIMARY KEY AUTOINCREMENT,
    user_name   TEXT NOT NULL,
    email       TEXT NOT NULL,
    password    TEXT NOT NULL
);

-- =====================
--   TABLE: Habits
-- =====================
CREATE TABLE IF NOT EXISTS Habits (
    habit_id    INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id     INTEGER NOT NULL,
    habit_name  TEXT NOT NULL,
    start_date  DATE NOT NULL,
    motivation  TEXT,
    is_active   INTEGER NOT NULL CHECK(is_active IN (0,1)), -- Bool implementation
    daily_goal  INTEGER,
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE
);

-- =====================
--   TABLE: DailyStatuses
-- =====================
CREATE TABLE IF NOT EXISTS DailyStatuses (
    status_id     INTEGER PRIMARY KEY AUTOINCREMENT,
    habit_id      INTEGER NOT NULL,
    dateTime      DATETIME NOT NULL,
    trigger       TEXT,
    note          TEXT,
    craving_level INTEGER,
    FOREIGN KEY (habit_id) REFERENCES Habits(habit_id) ON DELETE CASCADE
);

-- =====================
--   TABLE: SOSActions
-- =====================
CREATE TABLE IF NOT EXISTS SOSActions (
    action_id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id   INTEGER NOT NULL, -- Це поле є в PDF, але було відсутнє в попередньому коді
    text      TEXT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE
);

-- =====================
--   TABLE: UserSOSLogs
-- =====================
CREATE TABLE IF NOT EXISTS UserSOSLogs (
    log_id    INTEGER PRIMARY KEY AUTOINCREMENT,
    action_id INTEGER NOT NULL,
    user_id   INTEGER NOT NULL,
    dateTime  DATETIME NOT NULL,
    worked    INTEGER NOT NULL CHECK(worked IN (0,1)), -- Bool implementation
    FOREIGN KEY (action_id) REFERENCES SOSActions(action_id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE
);

-- =====================
--   TABLE: Quotes
-- =====================
CREATE TABLE IF NOT EXISTS Quotes (
    quote_id INTEGER PRIMARY KEY AUTOINCREMENT,
    text     TEXT NOT NULL
);