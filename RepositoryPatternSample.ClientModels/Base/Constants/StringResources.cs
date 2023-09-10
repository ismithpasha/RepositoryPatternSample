namespace RepositoryPatternSample.ClientModels.Base
{
	public class StringResources

	{
		public const string USER_NAME_REQUIRED = "User Name is required";
		public const string LOG_OUT = "Logged out";
		public const string TOKENS_MUST_BE_PROVIDED = "Tokens must be provided";
		public const string INVALID_TOKEN_DETAILS = "Invalid token details";
		public const string TOKEN_NOT_EXPIRED = "Token is not expired";
		public const string REFRESH_TOKEN_EXPIRED = "Refresh token expired";
		public const string EXCEPTIONAL_ERROR = "Exceptional Error";
		public const string USING_EMAIL_EXIST = "Email alredy exits";
		public const string INVALID_ID = "Invalid Id. Please provide valid object.";
		public const int TOKEN_EXPIRE_TIME = 540; // 2 in minutes
		public const int REFRESH_TOKEN_EXPIRE_TIME = 540; //10 im minutes
		public static string LOG_FILE_PATH => "Logs\\log- " + DateTime.Now.ToShortDateString() + ".txt";
		public const string DATA_FOUND = " Data Found";
		public const string DATA_EXIST = " Data Exist";
		public const string DATA_UPDATED = " Data Updated Successfully";
		public const string DATA_DELETE = " Deleted Successfully";
		public const string ERROR = "Error Occured";
		public const string SOMETHING_WENT_WRONG = "Something Went Wrong";
		public const string NO_DATA_FOUND = "No Data Found";
		public const string CREATE = " Successfully Created";
		public const string CREATE_FAILED = "Creation Failed";
		public const string ASSIGN = " Successfully Assigned";
		public const string ASSIGN_FAILED = " Successfully Failed";
		public const string EXECPTION = "Internal Server Error ";
		public const string SUCCESS = "Successfully updated";
		public const string DELETE_FAILED = " Failed to delete";
		public const string UPDATE_FAILED = " Failed to update";
		public const string NO_MENU_FOUND = " No Menu Found";
		public const string BAD_REQUEST_EXCEPTION_MESSAGE = "Invalied Request";
		public const string TRYING_ENTITY_ALREADY_EXIST = "Trying Entity already exist!!";
		public const string PLEASE_PROVIDE_ACCURATE_DATA = "Please provide valid data";
		public const string LOGIN_FAILED = "Login failed";
		public const string PLEASE_PROVIDE_THE_ID = "Invalid ID. Please provide valid Id";
		public const string PLEASE_GIVE_ACCURATE_ID = "Please give accurate id.";
		public static TimeSpan RedisabsoluteExpireTimeInSeconds => TimeSpan.FromSeconds(60);
		public static TimeSpan RedisabsoluteExpireTimeInMinutes => TimeSpan.FromMinutes(60);
		public static TimeSpan RedisabsoluteExpireTimeInHours => TimeSpan.FromHours(24);
		public static TimeSpan RedisabsoluteExpireTimeInDays => TimeSpan.FromHours(30);
		public const string USERNAME_EXIST = "User Name is Exist!";
		public const string IMAGE_PATH = "images";
		public const string NOT_EXIST = "Not Exist";
		public const string EXIST = "Exist";
	}
}
