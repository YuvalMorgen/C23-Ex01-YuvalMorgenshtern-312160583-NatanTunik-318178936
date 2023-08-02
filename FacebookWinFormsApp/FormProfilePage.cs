using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{
    public partial class FormProfilePage : Form
    {
        private readonly User r_LoggedInUser;
        private readonly Random r_Random = new Random();
        private readonly List<string> r_SelectedAlbumPhotos = new List<string>();
        private readonly List<Group> r_AllGroupsToGuess = new List<Group>();
        private Button m_CorrectGroupButton;
        private string m_CorrectGroupName;
        private int m_AlbumPhotoIndex;
        private int m_GroupsGameScore;
        private int m_GroupGameNumOfGames;

        public FormProfilePage(User i_LoggedInUser)
        {
            r_LoggedInUser = i_LoggedInUser;
            InitializeComponent();
            initProfile();
        }

        private void initProfile()
        {
            userNameText.Text = r_LoggedInUser.Name;
            profilePictureBox.Image = r_LoggedInUser.ImageNormal;
        }

        private void newPostBtn_Click(object sender, EventArgs e)
        {
            try
            {
                r_LoggedInUser.PostStatus(postTextBox.Text);
            }
            catch
            {
                MessageBox.Show(string.Format("The Post couldn't be uploaded to your page,{0} it was uploaded successfully to recent posts in post tab :)", Environment.NewLine));
            }

            recentPostsListBox.Items.Add(postTextBox.Text);
            postTextBox.Text = string.Empty;
        }

        private void albumsBtn_Click(object sender, EventArgs e)
        {
            albumsTab.Text = "Albums";
            fetchAlbums();
            tabController.SelectedTab = albumsTab;
        }

        private void fetchAlbums()
        {
            albumListBox.Items.Clear();
            albumListBox.DisplayMember = "Name";
            foreach (Album album in r_LoggedInUser.Albums)
            {
                albumListBox.Items.Add(album);
            }
        }

        private void displayPictureFromAlbum(string i_PictureAlbumURL)
        {
            if (i_PictureAlbumURL != null)
            {
                PictureDisplayer.LoadAsync(i_PictureAlbumURL);
            }
            else
            {
                PictureDisplayer.Image = PictureDisplayer.ErrorImage;
            }
        }

        private void fetchGroups()
        {
            groupsListBox.Items.Clear();
            groupsListBox.DisplayMember = "Name";
            foreach (Group group in r_LoggedInUser.Groups)
            {
                groupsListBox.Items.Add(group);
            }
        }

        private void groupsBtn_Click(object sender, EventArgs e)
        {
            groupsTab.Text = "Groups";
            fetchGroups();
            tabController.SelectedTab = groupsTab;
        }

        private void friendsBtn_Click(object sender, EventArgs e)
        {
            friendsTab.Text = "Friends";
            fetchFriends();
            tabController.SelectedTab = friendsTab;
        }

        private void fetchFriends()
        {
            friendsListBox.Items.Clear();
            friendsListBox.DisplayMember = "Name";
            try
            {
                foreach (User friend in r_LoggedInUser.Friends)
                {
                    friendsListBox.Items.Add(friend);
                }
            }
            catch
            {
                friendBirthdayTextBox.Text = "You don't have permission to do that :)";
            }
        }

        private void displayFriendProfilePicture()
        {
            if (friendsListBox.SelectedItems.Count == 1)
            {
                User selectedFriend = friendsListBox.SelectedItem as User;
                try
                {
                    FriendsPictureBox.Image = selectedFriend.ImageNormal;
                }
                catch
                {
                    MessageBox.Show("You don't have permission to do that :)");
                }
            }
        }

        private void backToLoginBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void eventsBtn_Click(object sender, EventArgs e)
        {
            eventsTab.Text = "Events";
            fetchEvents();
            tabController.SelectedTab = eventsTab;
        }

        private void fetchEvents()
        {
            eventsListBox.Items.Clear();
            eventsListBox.DisplayMember = "Name";
            foreach (Event e in r_LoggedInUser.Events)
            {
                eventsListBox.Items.Add(e);
            }
        }

        private void albumListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Album selectedAlbum = albumListBox.SelectedItem as Album;

            m_AlbumPhotoIndex = 0;
            r_SelectedAlbumPhotos.Clear();
            foreach (Photo photo in selectedAlbum.Photos)
            {
                r_SelectedAlbumPhotos.Add(photo.PictureNormalURL);
            }

            try
            {
                displayPictureFromAlbum(r_SelectedAlbumPhotos[m_AlbumPhotoIndex]);
            }
            catch
            {
                MessageBox.Show("Album can't be displayed");
            }
        }

        private void friendsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayFriendProfilePicture();
            displayFriendBirthday();
        }

        private void displayFriendBirthday()
        {
            User selectedFriend = friendsListBox.SelectedItem as User;

            try
            {
                friendBirthdayTextBox.Text = selectedFriend.Birthday;
            }
            catch
            {
                MessageBox.Show("You don't have permission to do that :)");
            }
        }

        private void groupsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Group selectedGroup = groupsListBox.SelectedItem as Group;

            try
            {
                groupsTextBox.Text = selectedGroup.WallPosts[0].Message;
            }
            catch
            {
                groupsTextBox.Text = "You don't have permission to do that :)";
            }
        }

        private void eventsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Event selectedEvent = eventsListBox.SelectedItem as Event;

            try
            {
                eventsPostTextBox.Text = selectedEvent.WallPosts[0].Message;
            }
            catch
            {
                eventsPostTextBox.Text = "You don't have permission to do that :)";
            }
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            if (m_AlbumPhotoIndex < r_SelectedAlbumPhotos.Count - 1)
            {
                m_AlbumPhotoIndex++;
                displayPictureFromAlbum(r_SelectedAlbumPhotos[m_AlbumPhotoIndex]);
            }
        }

        private void postsBtn_Click(object sender, EventArgs e)
        {
            postsTab.Text = "Posts";
            fetchPosts();
            tabController.SelectedTab = postsTab;
        }

        private void fetchPosts()
        {
            postsListBox.Items.Clear();
            postsListBox.DisplayMember = "Name";
            try
            {
                foreach (Post post in r_LoggedInUser.Posts)
                {
                    if (post.Message != null)
                    {
                        postsListBox.Items.Add(post.Message);
                    }
                }
            }
            catch
            {
                MessageBox.Show("You don't have permission to do that :)");
            }
        }

        private void nostalgicFeatureBtn_Click(object sender, EventArgs e)
        {
            nostalgiaTab.Text = "Nostalgia";
            fetchPosts();
            tabController.SelectedTab = nostalgiaTab;
        }

        private void nostalgicPostBtn_Click(object sender, EventArgs e)
        {
            List<Post> allPosts = new List<Post>();

            foreach (Post post in r_LoggedInUser.Posts)
            {
                if (post.Message != null)
                {
                    allPosts.Add(post);
                }
            }

            int randomIndex = r_Random.Next(0, allPosts.Count);
            Post chosenPost = allPosts[randomIndex];
            nostalgicPostDateBox.Text = string.Format("On {0} you posted:", chosenPost.UpdateTime.ToString());
            nostalgicPostBox.Text = chosenPost.Message;
        }

        private void prevBtn_Click(object sender, EventArgs e)
        {
            if (m_AlbumPhotoIndex >= 1)
            {
                m_AlbumPhotoIndex--;
                displayPictureFromAlbum(r_SelectedAlbumPhotos[m_AlbumPhotoIndex]);
            }
        }

        private void guessGroupFeatureBtn_Click(object sender, EventArgs e)
        {
            m_GroupsGameScore = 0;
            m_GroupGameNumOfGames = 0;
            guessTheGroupTab.Text = "Guess The Group";
            foreach (Group group in r_LoggedInUser.Groups)
            {
                r_AllGroupsToGuess.Add(group);
            }

            setRandomGroupToGuess();
            tabController.SelectedTab = guessTheGroupTab;
        }

        private void setRandomGroupToGuess()
        {
            List<Button> guessGroupBtns = new List<Button>() { groupGuessBtn1, groupGuessBtn2, groupGuessBtn3 };
            List<Group> chosenGroups = new List<Group>();
            int randomIndex = r_Random.Next(0, r_AllGroupsToGuess.Count);

            foreach (Button button in guessGroupBtns)
            {
                while (chosenGroups.Contains(r_AllGroupsToGuess[randomIndex]))
                {
                    randomIndex = r_Random.Next(0, r_AllGroupsToGuess.Count);
                }

                chosenGroups.Add(r_AllGroupsToGuess[randomIndex]);
                button.BackColor = Color.White;
                button.Text = r_AllGroupsToGuess[randomIndex].Name;
            }

            Group correctGroup = chosenGroups[r_Random.Next(0, chosenGroups.Count)];
            guessTheGroupPictureBox.Image = correctGroup.ImageLarge;
            m_CorrectGroupName = correctGroup.Name;
            m_CorrectGroupButton = guessGroupBtns[chosenGroups.IndexOf(correctGroup)];
        }

        private void updateGroupsGameResult(Button i_Button)
        {
            if (i_Button.Text == m_CorrectGroupName)
            {
                i_Button.BackColor = Color.Green;
                m_GroupsGameScore++;
            }
            else
            {
                i_Button.BackColor = Color.Red;
                m_CorrectGroupButton.BackColor = Color.Green;
                m_CorrectGroupButton.Update();
            }

            m_GroupGameNumOfGames++;
            i_Button.Update();
        }


        private void groupGuessBtn1_Click(object sender, EventArgs e)
        {
            updateGroupsGameResult(groupGuessBtn1);
            System.Threading.Thread.Sleep(1000);
            setRandomGroupToGuess();
        }

        private void groupGuessBtn2_Click(object sender, EventArgs e)
        {
            updateGroupsGameResult(groupGuessBtn2);
            System.Threading.Thread.Sleep(1000);
            setRandomGroupToGuess();
        }

        private void groupGuessBtn3_Click(object sender, EventArgs e)
        {
            updateGroupsGameResult(groupGuessBtn3);
            System.Threading.Thread.Sleep(1000);
            setRandomGroupToGuess();
        }

        private void endGuessGroupGame_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format("Game Over! {0} Your score is: * * * {1} * * *{2} Out of {3} games :)", Environment.NewLine, m_GroupsGameScore, Environment.NewLine, m_GroupGameNumOfGames));
            m_GroupsGameScore = 0;
            m_GroupGameNumOfGames = 0;
            tabController.SelectedTab = profileTab;
        }

        private void shareNostalgiaBtn_Click(object sender, EventArgs e)
        {
            string newPost = string.Format("{0}I found an old memory from {1}:{0}{2}", Environment.NewLine, nostalgicPostDateBox.Text, nostalgicPostBox.Text);
            MessageBox.Show(string.Format("Your nostalgic post was uploaded to your profile:{0}", newPost));
            recentPostsListBox.Items.Add(newPost);
        }
    }
}
