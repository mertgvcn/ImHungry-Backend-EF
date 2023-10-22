namespace WebAPI_Giris.Models
{
    public class Comment
    {
        public int commentID { get; set; }

        public int userID { get; set; }

        public int restaurantID { get; set; }

        public string text { get; set; }
    }
}
