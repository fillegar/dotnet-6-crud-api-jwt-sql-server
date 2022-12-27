using User.Core.Model;
using User.Repository;

namespace User.Service
{
    public class UserInfoService
    {
        private readonly UserInfoRepository _userInfoRepository;
        public UserInfoService(UserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        public async Task<UserInfo> getUserInfoByEmail(string email)
        {
            return await _userInfoRepository.getUserInfoByEmail(email);
        }

        public async Task<List<UserInfo>> getUserInfos()
        {
            return await _userInfoRepository.Get();
        }

        public void addUserInfo(UserInfo userInfo)
        {
            _userInfoRepository.Add(userInfo);
        }

        public async Task updateUserInfo(UserInfo userInfo)
        {
            await _userInfoRepository.Update(userInfo);
        }

        public void removeUserInfo(UserInfo userInfo)
        {
            _userInfoRepository.Remove(userInfo);
        }
    }
}
