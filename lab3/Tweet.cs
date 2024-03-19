namespace tweets {
    public class Tweet {

        public string? Text {get; set;}
        public string? UserName {get; set;}
        public string? LinkToTweet {get; set;}
        public string? FirstLinkUrl {get; set;}
        public string? CreatedAt {get; set;}
        public string? TweetEmbedCode {get; set;}

        public Tweet() {}
        public Tweet(string? text, string? userName, string? linkToTweet, string? firstLinkUrl, string? createdAt, string? tweetEmbedCode) {
            Text = text;
            UserName = userName;
            LinkToTweet = linkToTweet;
            FirstLinkUrl = firstLinkUrl;
            CreatedAt = createdAt;
            TweetEmbedCode = tweetEmbedCode;
        }

        public override string ToString() {
            return "Text: " + Text + ", UserName: " + UserName + ", LinkToTweet: " + LinkToTweet + ", FirstLinkUrl: " + FirstLinkUrl + ", CreatedAt: " + CreatedAt + ", TweetEmbedCode: " + TweetEmbedCode;
        }

        public string userAndDate() {
            return "UserName: " + UserName + ", CreatedAt: " + CreatedAt; 
        }
    }
}