using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class BoneTouchHandler : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject infoPanel;
    public TMP_InputField infoText;
    public Button closeButton;
    public float longPressDuration = 2f;

    private float pressTime;
    private bool isPressing;
    private Coroutine longPressCoroutine;

    void Start()
    {
        if (infoText != null && infoText.textComponent != null)
        {
            float dpiScale = Screen.dpi / 160f;
            int scaledFontSize = Mathf.RoundToInt(14 * dpiScale); 
            infoText.textComponent.fontSize = Mathf.Clamp(scaledFontSize, 5, 9);
        }

        infoPanel.SetActive(false);
        closeButton.onClick.AddListener(() => infoPanel.SetActive(false));
    }

    void Update()
    {
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        if (Touchscreen.current == null || Touchscreen.current.primaryTouch.press.isPressed == false)
            return;

        var touch = Touchscreen.current.primaryTouch;

        if (touch.press.wasPressedThisFrame)
        {
            isPressing = true;
            pressTime = Time.time;
            longPressCoroutine = StartCoroutine(CheckLongPress(touch));
        }
        else if (touch.press.wasReleasedThisFrame)
        {
            if (isPressing)
            {
                if (longPressCoroutine != null)
                    StopCoroutine(longPressCoroutine);
                isPressing = false;
            }
        }
    }

    IEnumerator CheckLongPress(InputControl touch)
    {
        yield return new WaitForSeconds(longPressDuration);

        if (isPressing)
        {
            Vector2 screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                string boneName = hit.transform.name;
                string info = GetBoneInfo(boneName);
                ShowInfo(info);
            }
        }
    }

    void ShowInfo(string text)
    {
        infoText.text = text;
        infoText.textComponent.characterSpacing = 10f;

        infoPanel.SetActive(true);
    }

    public string GetBoneInfo(string boneName)
    {
        switch (boneName)
        {
            case "WirbelS_ColumnaVertebralis":
                return "Columna vertebralis (Wirbelsäule): Die Wirbelsäule ist die zentrale Achse des Körpers und schützt das Rückenmark.";
            case "Schulterblatt_Scapulae":
                return "Scapulae (Schulterblatt): Die Schulterblätter verbinden Oberarm und Rumpf und ermöglichen eine große Bewegungsfreiheit des Arms.";
            case "Schienbein_Tibia":
                return "Tibia (Schienbein): Das Schienbein ist der kräftige vordere Knochen des Unterschenkels und trägt einen Großteil des Körpergewichts.";
            case "Fueße_OssaPedis":
                return "Ossa pedis (Fußknochen): Die Fußknochen bestehen aus mehreren Einzelknochen und bilden die Grundlage für Stand und Bewegung.";
            case "Oberarm_Humerus":
                return "Humerus (Oberarmknochen): Der Humerus ist der lange Knochen des Oberarms und verbindet Schulter und Ellenbogen.";
            case "Unterarm_Radius":
                return "Radius (Speiche): Die Speiche liegt auf der Daumenseite des Unterarms und ist besonders an der Drehung des Arms beteiligt.";
            case "Schaedl_Cranium":
                return "Cranium (Schädel): Der Schädel schützt das Gehirn und formt das Gesichtsskelett.";
            case "Becken_Pelvis":
                return "Pelvis (Becken): Das Becken verbindet die Wirbelsäule mit den Beinen und schützt innere Organe wie Blase und Darm.";
            case "Oberschaenkl_Femur":
                return "Femur (Oberschenkelknochen): Der Femur ist der längste und stärkste Knochen des Körpers und verbindet Becken und Knie.";
            case "Halswirbel_VertebraeCervicales":
                return "Vertebrae cervicales (Halswirbel): Die sieben Halswirbel tragen den Kopf und ermöglichen Bewegungen wie Nicken und Drehen.";
            case "Brustbein_Sternum":
                return "Sternum (Brustbein): Das Brustbein liegt in der Mitte des Brustkorbs und ist der Ansatzpunkt für die Rippen über den Knorpel.";
            case "Unterarm_Ulna":
                return "Ulna (Elle): Die Elle ist der zweite Unterarmknochen auf der Kleinfingerseite und bildet zusammen mit der Speiche das Ellenbogengelenk.";
            case "Haende_OssaManus":
                return "Ossa manus (Handknochen): Die Handknochen bestehen aus Handwurzel, Mittelhand und Fingergliedern und ermöglichen präzise Bewegungen.";
            case "Schluesslbein_Clavicula":
                return "Clavicula (Schlüsselbein): Das Schlüsselbein verbindet Brustbein und Schulterblatt und stabilisiert den Schultergürtel.";
            case "Kniescheibe_Patella":
                return "Patella (Kniescheibe): Die Kniescheibe schützt das Kniegelenk und verbessert die Hebelwirkung der Oberschenkelmuskulatur.";
            case "Wadenbein_Fibula":
                return "Fibula (Wadenbein): Das Wadenbein verläuft parallel zur Tibia, ist dünner und dient hauptsächlich der Muskelverankerung.";
            case "Rippen_Costae":
                return "Costae (Rippen): Die Rippen bilden den knöchernen Brustkorb und schützen Organe wie Herz und Lunge.";
            default:
                return "Keine Informationen verfügbar für: " + boneName;
        }
    }
}
