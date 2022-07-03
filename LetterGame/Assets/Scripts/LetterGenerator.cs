using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LetterGenerator : MonoBehaviour
{
    [Header("References")]
    public Transform Container;
    public TMP_Text letter;
    public Transform startPosition;
    public InputField RowInput;
    public InputField ColumnInput;
    GridLayoutGroup layout;



    [Header("Attributes")]
    private int numberOfRows = 4;
    private int numberOfColumn = 4;
    //Those are max values for fixed screen scaling
    private int maxRowValue = 9;
    private int maxColumnValue = 9;

    //It is just an option for spacing but better to use Grid Layout spacing on Container
    [SerializeField] private float rowSpace;
    [SerializeField] private float columnSpace;


    [Header("Datas")]
    string alphabetUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private List<string> generatedList = new List<string>();
    private List<TMP_Text> currentTexts = new List<TMP_Text>();
    


    private void Start()
    {
        layout = GetComponent<GridLayoutGroup>();

    }

    public void GenerateLetter()
    {

        RenewPreviousTexts();

        GetRowNColumn();

        //Do not change constraintCount it will auto get values from inputs and scale it to screen
        layout.constraintCount = numberOfColumn;
      

        for (int row = 0; row < numberOfRows; row++)
        {
            for (int col = 0; col < numberOfColumn; col++)
            {
                char randomLetter = alphabetUpper[Random.Range(0, alphabetUpper.Length)];
                Vector3 startingPos = new Vector3(startPosition.position.x + col * columnSpace, startPosition.position.y - row * rowSpace, startPosition.position.z);
                TMP_Text _letter = Instantiate(letter, startingPos, Quaternion.identity);
                _letter.transform.SetParent(Container.transform);
                _letter.text = randomLetter.ToString();
                
                //This will add generated letters to list so we can shuffle or remove all previous letters
                generatedList.Add(_letter.text);
                currentTexts.Add(_letter);

            }
        }
    }

    private void RenewPreviousTexts()
    {
        //If you generate new letters it will remove all previous ones
        if (currentTexts != null)
        {
            foreach (TMP_Text text in currentTexts)
            {
                Destroy(text.gameObject);
            }
            generatedList.Clear();
            currentTexts.Clear();
        }
    }
    private void GetRowNColumn()
    {
        //Convert inputs str to int
        int.TryParse(RowInput.text, out numberOfRows);
        int.TryParse(ColumnInput.text, out numberOfColumn);
        //Max row and column value fixed by 9 because it scales almost all screen with max values
        if (numberOfRows > maxRowValue) numberOfRows = maxRowValue;
        else if (numberOfColumn > maxColumnValue) numberOfColumn = maxColumnValue;
    }
    public void ShuffleLetters()
    {
        Shuffle(generatedList);
        for (int i = 0; i < currentTexts.Count; i++)
        {
            //Ýnstead of remove previous letters use them again
            currentTexts[i].text = generatedList[i]; 
        }
      
    }
  
    private void Shuffle<T>(List<T> letterList)
    {
        //Generic shuffle func
        for (int i = 0; i < letterList.Count; i++)
        {
            T temp = letterList[i];
            int rand = Random.Range(i, letterList.Count);
            letterList[i] = letterList[rand];
            letterList[rand] = temp;
        }
    }
}
