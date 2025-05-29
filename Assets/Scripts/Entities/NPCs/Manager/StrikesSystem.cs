using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// /// TheStrikesSystem class is responsible for handling the strikes that the manager can set to the player.
/// </summary>
public class StrikesSystem : MonoBehaviour
{
    [SerializeField]
    private Image strike1Image, strike2Image, strike3Image;

    /// <summary>
    /// The strikes attribute is used to store the number of strikes the player has.
    /// </summary>
    private int strikes = 0;

    /// <summary>
    /// The MAXSTRIKES attribute IS used to store  the maximum number of strikes the player can have, respectively.
    /// </summary>
    private const int MAX_STRIKES = 3;

    /// <summary>
    /// the DispatchStrike method is used to dispatch a strike to the player when the manager is attacked or when the player attacks a customer in the manager's field of view.
    /// </summary>
    /// <remarks>
    /// This method increments the number of the strikes and updtases the UI element that displays the number of strikes.
    /// If the number of strikes reaches the maximum number of strikes, the game is exited after a delay.
    /// If the manager is attacked, the KnockEntity component is used to knock the manager down, otherwise, 
    /// the LastCustomerAttacked in the EventManager instance is reset to null to avoid further strikes from being dispatched by the manager again.
    /// </remarks>
    /// <param name="managerAttacked">if set to <c>true</c> [manager attacked].</param>
    public void DispatchStrike(bool managerAttacked = false)
    {
        if (strikes >= MAX_STRIKES)
        {
            return;
        }

        strikes++;

        if (!managerAttacked)
        {
            EventManager.GetInstance().LastCustomerAttacked = null;
        }
        else
        {
            GetComponent<KnockEntity>().Knock(gameObject, GetComponent<Rigidbody>(), transform.position);
        }

        Color imageColor;

        switch (strikes)
        {
            case 1 :
                imageColor = strike1Image.color;

                imageColor.a = 1f;

                strike1Image.color = imageColor;

                break;

            case 2:
                imageColor = strike2Image.color;

                imageColor.a = 1f;

                strike2Image.color = imageColor;

                break;

            case 3 :
                imageColor = strike3Image.color;

                imageColor.a = 1f;

                strike3Image.color= imageColor;

                break;
            default:
                break;
        }

      
        if (strikes >= MAX_STRIKES)
        {   // Waits 3 seconds before exiting the game
            StartCoroutine(Utils.WaitAndExecute(3f, () => Utils.ExitGame()));
        }
    }
}
