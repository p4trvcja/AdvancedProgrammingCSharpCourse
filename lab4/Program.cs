using System.ComponentModel.Design;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

public class Reader<T> {
    public List<T> readList(String path, Func<String[], T> generate) {
        if(!File.Exists(path))
            throw new Exception("There is no such file");
        
        List<T> res = new List<T>();
        string[] lines = File.ReadAllLines(path).Skip(1).ToArray();

        foreach(var line in lines)
            res.Add(generate(line.Split(',')));

        return res;
    }
}

public class Program {
    public static void Main() {
        System.Console.WriteLine("------------ Exercise 1 ------------");
        List<Territory> territoriesList = new Reader<Territory>().readList("csv_data\\territories.csv", x => new Territory(x[0], x[1], x[2]));
        List<Employee> employeesList = new Reader<Employee>().readList("csv_data\\employees.csv", x => new Employee(x));
        List<Region> regionsList = new Reader<Region>().readList("csv_data\\regions.csv", x => new Region(x[0], x[1]));
        List<EmployeeTerritory> employeeTerritoriesList = new Reader<EmployeeTerritory>().readList("csv_data\\employee_territories.csv", x => new EmployeeTerritory(x[0], x[1]));

        System.Console.WriteLine($"Territories number: {territoriesList.Count()}");
        System.Console.WriteLine($"Employees number: {employeesList.Count()}");
        System.Console.WriteLine($"Regions number: {regionsList.Count()}");
        System.Console.WriteLine($"employeeTerritories number: {employeeTerritoriesList.Count()}");

        System.Console.WriteLine("------------ Exercise 2 ------------");
        var employeesLastnameQuery = from e in employeesList select e.lastname;

        foreach(var e in employeesLastnameQuery)
            System.Console.WriteLine(e);

        System.Console.WriteLine("------------ Exercise 3 ------------");
        var employeesTerritoryRegionQuery = from e in employeesList
                join et in employeeTerritoriesList on e.employeeid equals et.employeeid
                join t in territoriesList on et.territoryid equals t.territoryid
                join r in regionsList on t.regionid equals r.regionid
            select new {
                e.lastname, 
                territory = t.territorydescription, 
                region = r.regiondescription
            };

        foreach(var e in employeesTerritoryRegionQuery.ToList())
            System.Console.WriteLine(e); 
        
        System.Console.WriteLine("------------ Exercise 4 ------------");
        var regionsAndLastnamesQuery = from r2 in (
            from r in regionsList
                join t in territoriesList on r.regionid equals t.regionid
                join et in employeeTerritoriesList on t.territoryid equals et.territoryid
                join e in employeesList on et.employeeid equals e.employeeid
            select new {
                region = r, 
                employee = e
            })
            group r2 by r2.region into grouped
            select grouped;
        
        foreach(var r in regionsAndLastnamesQuery) {
            System.Console.WriteLine($"{r.Key.regiondescription}:");
            var employeeList = r.Distinct().ToList();
            foreach(var em in employeeList)
                System.Console.WriteLine($"    {em.employee.lastname}");
        }

        System.Console.WriteLine("------------ Exercise 5 ------------");
        var query4 = from r2 in (
            from r in regionsList
                join t in territoriesList on r.regionid equals t.regionid
                join et in employeeTerritoriesList on t.territoryid equals et.territoryid
                join e in employeesList on et.employeeid equals e.employeeid
            select new {
                region = r, 
                employee = e
            })
            group r2 by r2.region into grouped
            select new {
                grouped.Key, 
                count = grouped.Distinct().Count()
            };

        foreach(var r in query4) {
            System.Console.WriteLine($"{r.Key.regiondescription}: {r.count}");
        }

        System.Console.WriteLine("------------ Exercise 6 ------------");
        List<Order> ordersList = new Reader<Order>().readList("csv_data\\orders.csv", x => new Order(x));
        List<OrderDetails> orderDetailsList = new Reader<OrderDetails>().readList("csv_data\\orders_details.csv", x => new OrderDetails(x));

        var query5 = from e2 in (
            from e in employeesList
                join o in ordersList on e.employeeid equals o.employeeid
                join od in orderDetailsList on o.orderid equals od.orderid
            select new {
                employee = e, 
                orderTotal = Double.Parse(od.unitprice, CultureInfo.InvariantCulture) * int.Parse(od.quantity) * (1 - Double.Parse(od.discount, CultureInfo.InvariantCulture))
            })
            group e2 by e2.employee into grouped
            select new {
                employee = grouped.Key, 
                count = grouped.Count(), 
                average = grouped.Average(o => o.orderTotal), 
                max = grouped.Max(o => o.orderTotal)
            };

        foreach(var e in query5) {
            System.Console.WriteLine($"Employee: {e.employee.lastname}");
            System.Console.WriteLine($"   number of orders: {e.count}");
            System.Console.WriteLine($"   average orders value: {e.average}");
            System.Console.WriteLine($"   max order value: {e.max}");
        }
        
    }
}