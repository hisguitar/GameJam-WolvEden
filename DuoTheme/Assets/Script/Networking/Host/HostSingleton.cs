using UnityEngine;

public class HostSingleton : SingletonPersistent<HostSingleton>
{
    public HostGameManager GameManager { get; private set; }
    
    private void OnDestroy()
    {
        GameManager?.Dispose();
    }

    public void CreateHost()
    {
        GameManager = new HostGameManager();
    }

    public void TestScript()
    {
        Debug.Log("Host singleton is work");
    }
}