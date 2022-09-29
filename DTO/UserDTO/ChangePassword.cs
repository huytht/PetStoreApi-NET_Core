namespace PetStoreApi.DTO.UserDTO
{
    public class ChangePassword
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public ChangePassword()
        {
        }
        public ChangePassword(string Username, string OldPassword, string NewPassword)
        {
            this.Username = Username;
            this.OldPassword = OldPassword;
            this.NewPassword = NewPassword;
        }
    }
}
