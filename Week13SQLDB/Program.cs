
using System.Data;
using System.Data.SQLite;

readData(createConnection());
//InsertCustomer(createConnection());
//removeCustomer(createConnection());
findCustomer(createConnection());

static SQLiteConnection createConnection()
{
    SQLiteConnection connection = new SQLiteConnection("Data Source = mydb.db; Version=3; new = true; Compress = true");

    try
    {
        connection.Open();
        Console.WriteLine("DB found");
    }
    catch
    {
        Console.WriteLine("DB not found");
    }

    return connection;
}

static void readData(SQLiteConnection myConnection)
{
    Console.Clear();
    SQLiteDataReader reader;
    SQLiteCommand command;

    command = myConnection.CreateCommand();
    command.CommandText = "Select rowid, * from customer";

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowId = reader["rowid"].ToString();
        string readerStringFirstname = reader.GetString(1);
        string readerStringLastname = reader.GetString(2);
        string readerStringDoB = reader.GetString(3);

        Console.WriteLine($"{readerRowId}. Full name: {readerStringFirstname} {readerStringLastname}; DoB {readerStringDoB}");
    }
    myConnection.Close();
}

static void InsertCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;
    string FName, LName, dob;

    Console.WriteLine("Enter first name:");
    FName = Console.ReadLine();
    Console.WriteLine("Enter last name:");
    LName = Console.ReadLine();
    Console.WriteLine("Enter date of birth (mm-dd-yyy):");
    dob = Console.ReadLine();


    command = myConnection.CreateCommand();
    command.CommandText = $"INSERT INTO Customer(firstName, lastName, dateOfBirth) " +
        $"VALUES ('{FName}', '{LName}', '{dob}')";
    
    int rowInserted = command.ExecuteNonQuery();
    Console.WriteLine($"Row inserted: {rowInserted}");

    

    readData(myConnection);

}


static void removeCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;

    string idToDelete;
    Console.WriteLine("Enter an ID to delete a customer");
    idToDelete = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"Delete From customer WHERE rowid = {idToDelete}";
    int rowRemoved = command.ExecuteNonQuery();
    Console.WriteLine($"{rowRemoved} was removed from the table customer");

    readData(myConnection);
}

static void findCustomer(SQLiteConnection myConnection)
{
    SQLiteDataReader reader;
    SQLiteCommand command;
    string searchName;
    Console.WriteLine("Enter a first name to display customer data:");

    searchName = Console.ReadLine();
    command = myConnection.CreateCommand();
    command.CommandText = $"SELECT customer.rowid, customer.firstName, customer.lastName, status.statusType " +
    $"FROM customerStatus " +
    $"JOIN customer ON customer.rowid = customerStatus.customerId " +
    $"JOIN status ON status.rowid = customerStatus.statusId " +
    $"WHERE firstname LIKE '{searchName}'";
    reader = command.ExecuteReader();

    while(reader.Read())
    {
        string readerRowid = reader["rowid"].ToString();
        string readerStringName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringStatus = reader.GetString(3);
        Console.WriteLine($"Search result: ID: {readerRowid}. {readerStringName} {readerStringLastName}. Status: {readerStringStatus}");
    }
    
    myConnection.Close();
}

