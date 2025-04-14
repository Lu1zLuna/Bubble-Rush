using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        public static GameManager instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;

            // WinMenu.SetActive(false);
            // LoseMenu.SetActive(false);
            // levelsUI.SetActive(false);
            // sequenceBubbles = new List<Transform>();
            // connectedBubbles = new List<Transform>();
            // bubblesToDrop = new List<Transform>();
            // bubblesToDissolve = new List<Transform>();
            // DontDestroyOnLoad(gameObject);
        }
        #endregion
    }
}