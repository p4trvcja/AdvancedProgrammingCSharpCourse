using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace sqlite {
    public class Program {
        private static (List<List<object>>, List<string>) readCSV(string filename, char delimiter) {
            if(!File.Exists(filename)) {
                throw new FileNotFoundException("File not found.");
            }
            string[] lines = File.ReadAllLines(filename);

            List<List<object>> database = new List<List<Object>>();
            List<string> header = lines[0].Split(delimiter).ToList();

            foreach(string line in lines.Skip(1)) {
                string[] values = line.Split(delimiter);
                List<object> row = new List<object>();

                foreach(var val in values) {
                    if(string.IsNullOrWhiteSpace(val)) {
                        row.Add(null);
                    } else if(double.TryParse(val, CultureInfo.InvariantCulture, out double doubleVal)) {
                        row.Add(doubleVal);
                    } else if(bool.TryParse(val, out bool boolVal)) {
                        row.Add(boolVal);
                    } else {
                        row.Add(val);
                    }
                }
                database.Add(row);
            }

            for (int i = 0; i < header.Count; i++) {
                bool isIntColumn = true;
                int intValue = 0;

                foreach(List<object>? row in database)
                    if(row != null && !int.TryParse(row[i]?.ToString(), out intValue)) {
                        isIntColumn = false;
                        break;
                    }
                
                if(isIntColumn) {
                    for(int j = 0; j < database.Count; j++)
                        if(database[j] != null) 
                            database[j][i] = int.Parse(database[j][i].ToString());
                }
            }
            
            return (database, header);
        }

        private static Dictionary<string, Tuple<Type, bool>> columnTypes(List<List<object>> database, List<string> header) {
            Dictionary<string, Tuple<Type, bool>> columnInfo = new Dictionary<string, Tuple<Type, bool>>();

            foreach(var col in header) {
                List<object> column = database.Select(row => row[header.IndexOf(col)]).ToList();
                bool nullable = column.Contains(null);

                Type type = column.First(value => value != null).GetType();
                if (column.All(value => value == null || value.GetType() == type))
                    columnInfo.Add(col, new Tuple<Type, bool>(type, nullable));
                else
                    columnInfo.Add(col, new Tuple<Type, bool>(typeof(string), nullable));
            }
            return columnInfo;
        }

        
        private static bool createTable(Dictionary<string, Tuple<Type, bool>> dataInfo, string tableName, SqliteConnection connection) {
            try {
                SqliteCommand delTableCmd = connection.CreateCommand();
                delTableCmd.CommandText = "DROP TABLE IF EXISTS " + tableName;
                delTableCmd.ExecuteNonQuery();

                SqliteCommand createTableCmd = connection.CreateCommand();
                string createCommand = "CREATE TABLE " + tableName + " (";
                
                int totalColumns = dataInfo.Count;
                int currentIndex = 0;
                foreach(var row in dataInfo) {
                    string type;

                    if(row.Value.Item1 == typeof(Int32)) 
                        type = "INTEGER";
                    else if(row.Value.Item1 == typeof(Double))
                        type = "REAL";
                    else if(row.Value.Item1 == typeof(Boolean))
                        type = "BOOLEAN";
                    else 
                        type = "TEXT";

                    createCommand += $"{row.Key} {type}";
                    if(!row.Value.Item2) createCommand += " NOT NULL";
                    if (currentIndex < totalColumns - 1)
                        createCommand += ", ";
                    else
                        createCommand += ");";
                    currentIndex++;
                }
                System.Console.WriteLine(createCommand);
                createTableCmd.CommandText = createCommand;              
                createTableCmd.ExecuteNonQuery();
                return true;
            } catch(Exception e) { 
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private static bool insertData(List<List<object>> data, List<string> header, string tableName, SqliteConnection connection) {
            try {
                SqliteCommand insertCmd = connection.CreateCommand();

                foreach(var row in data) {
                    string insertCommand = "INSERT INTO " + tableName + " VALUES (";
                    foreach(var val in row) {
                        if(val == null)
                            insertCommand += "NULL, ";
                        else if(val.GetType() == typeof(string))
                            insertCommand += $"\'{val}\', ";
                        else if(val.GetType() == typeof(double))
                            insertCommand += ((double)val).ToString("0.0", CultureInfo.InvariantCulture) + ", ";
                        else
                            insertCommand += $"{val}, ";
                    }
                    insertCommand = insertCommand.TrimEnd(',', ' ') + ")";

                    System.Console.WriteLine(insertCommand);
                    insertCmd.CommandText = insertCommand;
                    insertCmd.ExecuteNonQuery();
                }
                return true;
                
            } catch(Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private static void selectAll(string tableName, SqliteConnection connection) {
            SqliteCommand selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM " + tableName + ";";
            System.Console.WriteLine(selectCmd.CommandText);

            using(SqliteDataReader reader = selectCmd.ExecuteReader()) {
                for(int i = 0; i < reader.FieldCount; i++) {
                    Console.Write(reader.GetName(i));
                    if(i < reader.FieldCount - 1)
                        Console.Write(", ");
                }
                Console.WriteLine();

                while(reader.Read()) {
                    for (int i = 0; i < reader.FieldCount; i++) {
                        Console.Write(reader.GetValue(i));
                        if( i < reader.FieldCount - 1)
                            Console.Write(", ");
                    }
                    Console.WriteLine();
                }
            }
        }

        
        public static void Main(string[] args) {
            string filename = "database.csv";
            char delimiter = ',';
                        
            Console.WriteLine("--------- Exercise 1 ---------");
            (List<List<Object>> database, List<string> header) = readCSV(filename, delimiter);

            Console.Write("Header: ");
            foreach(var s in header) {
                Console.Write(s + " ");
            }
            Console.WriteLine();

            Console.WriteLine("--------- Exercise 2 ---------");
            Dictionary<string, Tuple<Type, bool>> columnInfo = columnTypes(database, header);

            foreach (var column in columnInfo) {
                Console.WriteLine($"Column: {column.Key}, Type: {column.Value.Item1.Name}, Nullable: {column.Value.Item2}");
            }

            Console.WriteLine("--------- Exercise 3 ---------");
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "./database.db";

            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString)) {
                connection.Open();
                bool created = createTable(columnInfo, "testTable", connection);
                Console.WriteLine(created? "Table created successfully." : "An error occured. The table was not created.");
            
            Console.WriteLine("--------- Exercise 4 ---------");
                connection.Open();
                bool inserted = insertData(database, header, "testTable", connection);
                Console.WriteLine(inserted? "Values inserted successfully." : "An error occured. The values were not inserted.");

            Console.WriteLine("--------- Exercise 5 ---------");
                connection.Open();
                selectAll("testTable", connection);
            }

        }
    }
}
