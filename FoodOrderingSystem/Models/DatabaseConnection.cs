using System;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
namespace FoodOrderingSystem.Models;
using System.Collections;
using System.Threading;

public class DatabaseConnection
{

    public string UserDataConnection(User user)
    {
        string message = registerationValidation(user);

        if (message == "ok")
        {
            using (SqlConnection userConnection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
            {
                userConnection.Open();
                SqlCommand insertcommand = new SqlCommand($"insert into FoodApp1 (UserName,Password,Email,MobileNumber) values ('{user.UserName}','{user.Password}','{user.Email}','{user.MobileNumber}')", userConnection);
                insertcommand.Parameters.AddWithValue("@UserName", user.UserName);
                insertcommand.Parameters.AddWithValue("@Password", user.Password);
                insertcommand.Parameters.AddWithValue("@Email", user.Email);
                insertcommand.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
                insertcommand.ExecuteNonQuery();

            }
        }

        return message;
    }

    public string FeedBackConnection(FeedBack user)
    {
        string message = FeedBackValidation(user);
        if (message == "ok")
        {
            using (SqlConnection userConnection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
            {
                userConnection.Open();
                SqlCommand insertcommand = new SqlCommand($"insert into Feedback (UserName,EMail,Rating,Message) values ('{user.UserName}','{user.EMail}','{user.Rating}','{user.Message}')", userConnection);
                // insertcommand.Parameters.AddWithValue("@UserName", user.UserName);
                // insertcommand.Parameters.AddWithValue("@EMail", user.EMail);
                // insertcommand.Parameters.AddWithValue("@Rating", user.Rating);
                // insertcommand.Parameters.AddWithValue("@Message", user.Message);
                insertcommand.ExecuteNonQuery();

            }

        }

        return message;
    }


    public string FeedBackValidation(FeedBack user)
    {
        string message = "";
        if (user.UserName != null)
        {
            Regex UserNamepattern = new Regex("^[A-Z][A-Za-z0-9]{5,}$");
            Regex Emailpattern = new Regex("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$");
            if (UserNamepattern.IsMatch(user.UserName) && (Emailpattern.IsMatch(user.EMail)))
            {

                message = "ok";
            }

            else
            {
                message = "Invalid Password";
            }

        }
        else
        {
            message = "Empty";
        }
        return message;
    }

    public string registerationValidation(User user)
    {
        string message = "";
        if (user.UserName != null)
        {
            Regex UserNamepattern = new Regex("^[A-Z][A-Za-z0-9]{5,}$");
            Regex Passwordpattern = new Regex("^[A-Z][A-Za-z0-9_@#$%^&*!]{6,}$");
            Regex Emailpattern = new Regex("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$");
            Regex MobileNumberpattern = new Regex("^[6-9]{1}[0-9]{9}");


            if (String.Equals(user.Password, user.RepeatPassword))
            {
                if (UserNamepattern.IsMatch(user.UserName) && (Passwordpattern.IsMatch(user.Password)) && (Emailpattern.IsMatch(user.Email)) && (MobileNumberpattern.IsMatch(user.MobileNumber)))
                {

                    message = "ok";
                }
                else
                {
                    message = "Invalid Password";
                }
            }
            else
            {
                message = "Password dosent match";
            }

            return message;
        }



        else
        {
            message = "empty";
        }
        return message;
    }




    public string loginValidation(LoginUser loginuser)
    {
        string info = "";
        using (SqlConnection userConnection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
        {
            userConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("select Password from FoodApp1 where UserName = '" + loginuser.UserName + "';", userConnection);
            string password = (string)sqlCommand.ExecuteScalar();
            if (password != null && password == loginuser.Password)
            {

                if(loginuser.UserName == "Admin123")
                {
                    info="admin";
                }
                else{
                    info = "done";
                }
               

            }
            else
            {
                info = "not done";
            }
            return info;
        }

    }

    public class SampleException : Exception
    {
        public SampleException(string Message) : base(Message)
        {

        }
    }


    public static DataTable getMenu()
    {
        SqlConnection connection = new SqlConnection("Data Source=ASPLAP1825\\SQLEXPRESS;Initial Catalog=FoodApp;Integrated Security=SSPI");
        DataTable dataTable1 = new DataTable();
        try
        {

            SqlCommand getcommand = new SqlCommand($"select * from Menu", connection);
            SqlDataAdapter dataadapter = new SqlDataAdapter(getcommand);
            dataadapter.Fill(dataTable1);

            // if(true){
            //     throw (new SampleException("This is a Sample UserDefined Exception"));
            // }

        }
        catch (SampleException sampleException)
        {
            Console.WriteLine("Sample Exception :" + sampleException.Message);
        }
        catch (FormatException formatException)
        {
            Console.WriteLine(formatException.Message);
        }
        catch (SqlException sqlException)
        {
            Console.WriteLine(sqlException.Message);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
        finally
        {
            connection.Close();
        }


        return dataTable1;
    }


    public bool AddToCart(string ItemName, string ItemPrice, string Username)
    {
        using (SqlConnection connection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
        {
            connection.Open();
            try
            {
                SqlCommand insertcommand = new SqlCommand("insert into CartDetails (ItemName,ItemPrice,UserName,Quantity) values (@item,@price,@user,@qty) ", connection);
                insertcommand.Parameters.AddWithValue("@item", ItemName);
                insertcommand.Parameters.AddWithValue("@price", ItemPrice);
                insertcommand.Parameters.AddWithValue("@user", Username);
                insertcommand.Parameters.AddWithValue("@qty", "1");


                insertcommand.ExecuteNonQuery();
                return true;
            }
            catch (SqlException sqlException)
            {
                return false;
            }

        }


    }

    public List<Cart> FetchCartItems(string name)
    {
        List<Cart> details = new List<Cart>();

        using (SqlConnection connection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
        {
            connection.Open();
            SqlCommand search = new SqlCommand("select * from CartDetails where UserName = @name", connection);
            search.Parameters.AddWithValue("@name", name);

            SqlDataReader reader = search.ExecuteReader();
            while (reader.Read())
            {
                Cart cart = new Cart();
                cart.ItemName = reader.GetString(0);
                cart.ItemPrice = reader.GetString(1);
                cart.Quantity=reader.GetString(3);
                cart.UserName = name;

                details.Add(cart);
            }
            reader.Close();


        }

        return details;
    }


    public void DeleteCartItemsByName(string name){
         using (SqlConnection connection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
        {
            connection.Open();
            SqlCommand search = new SqlCommand("delete from CartDetails where UserName = @name", connection);
            search.Parameters.AddWithValue("@name", name);

           search.ExecuteNonQuery();
        } 
    }

   

    



    public void DeleteCart(string ItemName)
    {
        using (SqlConnection connection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
        {
            connection.Open();
            SqlCommand deletecommand = new SqlCommand("delete from CartDetails where ItemName=@item", connection);
            deletecommand.Parameters.AddWithValue("@item", ItemName);


            deletecommand.ExecuteNonQuery();
        }
    }


    public void UpdateQuantity(string ItemName, string username, int Quantity, string buttn)
    {
       

        if (buttn == "+")
        {
            Quantity += 1;
        }
        else if (buttn == "-" && Quantity >1 )
        {
            Quantity -= 1;
        }

        using (SqlConnection connection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
        {
            connection.Open();
            SqlCommand updatecommand = new SqlCommand($"update CartDetails set Quantity='{Quantity}' where UserName='{username}' and ItemName='{ItemName}'",connection);
            updatecommand.ExecuteNonQuery();
        }
    }


   public int ApplyCoupon(string CouponCode,string UserName,int total)
    {
         SqlConnection connection = new SqlConnection("Data Source=  ASPLAP1825\\SQLEXPRESS;Initial Catalog=FoodApp;Integrated Security=SSPI");
         connection.Open();
         SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM OrderHistory WHERE UserName = @UserName", connection);
         cmd.Parameters.AddWithValue("@UserName", UserName); 
         int orderCount = (int)cmd.ExecuteScalar();

        // Check if the user is a new user and apply discount if applicable
        if (orderCount == 0)
        {
            SqlCommand couponCmd = new SqlCommand("SELECT DiscountPercentage FROM Coupons WHERE CouponCode = @CouponCode", connection);
            couponCmd.Parameters.AddWithValue("@CouponCode", CouponCode); // replace couponCode with the user's entered coupon code
            int discountPercentage = (int)couponCmd.ExecuteScalar();

            int discountAmount = (total * discountPercentage) / 100;
        
        return discountAmount ;

        }
    return 0;

    }
    
    

    public static DataTable FetchQuantity()
    {
        SqlConnection connection = new SqlConnection("Data Source=ASPLAP1825\\SQLEXPRESS;Initial Catalog=FoodApp;Integrated Security=SSPI");
        DataTable quantityDataTable = new DataTable();
        SqlCommand fetchcommand = new SqlCommand($"select * from CartDetails", connection);
        SqlDataAdapter dataadapter1 = new SqlDataAdapter(fetchcommand);
        dataadapter1.Fill(quantityDataTable);
        return quantityDataTable;
        
    }


        public IEnumerable FeedBackDetails()
            {
            List<FeedBack> feedback=new List<FeedBack>();

            using (SqlConnection connection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
                {
                    connection.Open();
                    
                    SqlCommand take = new SqlCommand("select * from Feedback",connection);
                    SqlDataReader dataReader = take.ExecuteReader();
                    while (dataReader.Read())
                    {
                    FeedBack feedback1=new FeedBack();
                    feedback1.UserName=dataReader.GetString(0);
                    feedback1.EMail=dataReader.GetString(1);
                        feedback1.Rating=dataReader.GetString(2);
                        feedback1.Message=dataReader.GetString(3);
                    feedback.Add(feedback1);
                }
                dataReader.Close();
                }
                return feedback;
            }

             public IEnumerable UserDetails()
            {
            List<RegisteredUsers> userdetails=new List<RegisteredUsers>();

            using (SqlConnection connection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
                {
                    connection.Open();
                    
                    SqlCommand command = new SqlCommand("select * from FoodApp1",connection);
                    SqlDataReader dataReader1 = command.ExecuteReader();
                    while (dataReader1.Read())
                    {
                  
                    RegisteredUsers registerusers=new RegisteredUsers();
                    registerusers.UserName=dataReader1.GetString(0);
                    registerusers.Password=dataReader1.GetString(1);
                    registerusers.Email=dataReader1.GetString(2);
                    registerusers.MobileNumber=dataReader1.GetString(3);
                    userdetails.Add(registerusers);
                }
                dataReader1.Close();
                }
                return userdetails;
            }


             public IEnumerable AdminOrder()
            {
            List<AdminOrders> adminOrderHistory=new List<AdminOrders>();

            using (SqlConnection connection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
                {
                    connection.Open();
                    
                    SqlCommand command = new SqlCommand("select * from OrderHistory",connection);
                    SqlDataReader dataReader2 = command.ExecuteReader();
                    while (dataReader2.Read())
                    {
                  
                    AdminOrders adminorders=new AdminOrders();
                    adminorders.UserName=dataReader2.GetString(2);
                    adminorders.ItemName=dataReader2.GetString(3);
                    adminorders.Quantity=dataReader2.GetString(4);
                    adminorders.ItemPrice=dataReader2.GetString(5);
                    adminorders.Total=dataReader2.GetString(6);
                    adminorders.Location=dataReader2.GetString(7);
                    adminorders.DateTime=dataReader2.GetString(1);

                    adminOrderHistory.Add(adminorders);
                }
                dataReader2.Close();
                }
                return adminOrderHistory;
            }


    public IEnumerable<OrderHistory> ViewOrderHistoryofUser(string name)
    {
    List<OrderHistory> history=new List<OrderHistory>();

    using (SqlConnection connection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
        {
            connection.Open();
            
            SqlCommand take = new SqlCommand("select * from OrderHistory where UserName = @name",connection);
            take.Parameters.AddWithValue("@name",name);
            SqlDataReader dataReader = take.ExecuteReader();
            while (dataReader.Read())
            {
                OrderHistory orderHistory = new OrderHistory();
                orderHistory.OrderId=dataReader.GetString(1);
                orderHistory.ItemName=dataReader.GetString(3);
                orderHistory.Quantity=dataReader.GetString(4);
                orderHistory.ItemPrice=dataReader.GetString(5);
                orderHistory.Total=dataReader.GetString(6);
                 orderHistory.Location=dataReader.GetString(7);
               history.Add(orderHistory);
            }
        dataReader.Close();
        }
        return history;
    }


    //Adding OrderId to Database
    public void addOrderId(string name,string orderId){
        Console.WriteLine("--------------------------");
         using (SqlConnection connection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
        {
            connection.Open();
            SqlCommand insert = new SqlCommand("insert into Orders values(@name,@id)",connection);
            insert.Parameters.AddWithValue("@name",name);
            insert.Parameters.AddWithValue("@id",orderId);

            insert.ExecuteNonQuery();
        }
    }


    public IEnumerable<Orders> FetchOrders(){
        List<Orders> orders=new List<Orders>();

        using (SqlConnection connection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
        {
            connection.Open();
            
            SqlCommand take = new SqlCommand("select * from Orders",connection);
            SqlDataReader dataReader = take.ExecuteReader();
            while (dataReader.Read())
            {
                Orders order = new Orders();
                order.UserName = dataReader.GetString(0);
                order.OrderId = dataReader.GetString(1);
                orders.Add(order);
            }
            dataReader.Close();
        }
        return orders;
    }


    public void AddItem(AddItems items)
    {
        using (SqlConnection userConnection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
             {
                userConnection.Open();
                SqlCommand addcommand = new SqlCommand($"insert into Menu (ItemName,ItemPrice,Category,ItemImage) values ('{items.ItemName}','{items.ItemPrice}','{items.Category}','{items.ItemImage}')", userConnection);
                addcommand.Parameters.AddWithValue("@ItemName", items.ItemName);
                addcommand.Parameters.AddWithValue("@ItemPrice", items.ItemPrice);
                addcommand.Parameters.AddWithValue("@Category", items.Category);
                addcommand.Parameters.AddWithValue("@ItemImage", items.ItemImage);
                addcommand.ExecuteNonQuery();
                }
    }

    public void DeleteItem(string ItemName)
    {
        using (SqlConnection userConnection = new SqlConnection("Data source =ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
             {
                userConnection.Open();
                SqlCommand deletecommand = new SqlCommand($"DELETE From Menu WHERE ItemName=@ItemName", userConnection);
                deletecommand.Parameters.AddWithValue("@ItemName",ItemName);
                deletecommand.ExecuteNonQuery();
             }
    }

     public void UpdateItem(string ItemName,string ItemPrice,string ItemImage)
    {
        using (SqlConnection userConnection = new SqlConnection("Data source = ASPLAP1825\\SQLEXPRESS; Database=FoodApp; Integrated security = SSPI;"))
             {
                userConnection.Open();
                 SqlCommand updatecommand = new SqlCommand($"UPDATE Menu SET ItemPrice=@ItemPrice,ItemImage=@ItemImage WHERE  ItemName=@ItemName", userConnection);
                updatecommand.Parameters.AddWithValue("@ItemName",ItemName);
                updatecommand.Parameters.AddWithValue("@ItemPrice",ItemPrice);
                updatecommand.Parameters.AddWithValue("@ItemImage",ItemImage);
                updatecommand.ExecuteNonQuery();

}
    }

}



