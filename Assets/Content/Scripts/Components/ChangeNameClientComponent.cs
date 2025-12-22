using TMPro;

namespace Game.Components
{
    public class ChangeNameClientComponent : ClientNetworkComponent
    {
        private TMP_Text _userNameText;
        private string _userName;

        public void Configure(TMP_Text userNameText)
        {
            _userNameText = userNameText;
        }

        private void OnUpdateNameView()
        {
            //_userNameText.text = e.UserName;
        }
    }
}