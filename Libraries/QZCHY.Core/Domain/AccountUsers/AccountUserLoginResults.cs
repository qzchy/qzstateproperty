namespace QZCHY.Core.Domain.AccountUsers
{
    /// <summary>
    /// 用户注册结果
    /// </summary>
    public enum AccountUserLoginResults
    {
        /// <summary>
        /// Login successful
        /// </summary>
        Successful = 1,
        /// <summary>
        /// AccountUser dies not exist (email or username)
        /// </summary>
        AccountUserNotExist = 2,
        /// <summary>
        /// Wrong password
        /// </summary>
        WrongPassword = 3,
        /// <summary>
        /// Account have not been activated
        /// </summary>
        NotActive = 4,
        /// <summary>
        /// AccountUser has been deleted 
        /// </summary>
        Deleted = 5,
        /// <summary>
        /// AccountUser not registered 
        /// </summary>
        NotRegistered = 6,
    }
}
