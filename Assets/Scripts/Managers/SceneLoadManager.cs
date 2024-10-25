using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : MonoBehaviour
{
    #region Singleton
    public static SceneLoadManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("<color=#A9A9A9>UI</color>")]
    [SerializeField] private float _fadeTime = 0.375f;
    [SerializeField] private Image _bgImage;
    [SerializeField] private Image _loadBarBG;
    [SerializeField] private Image _loadBarFill;
    [SerializeField] private TextMeshProUGUI _stateText;

    private bool _isLoading;

    private void Start()
    {
        _bgImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        _loadBarFill.fillAmount = 0.0f;

        _bgImage.enabled = false;
        _loadBarBG.enabled = false;
        _loadBarFill.enabled = false;
        _stateText.enabled = false;
    }

    public void LoadSecenAsync(string scene)
    {
        if (_isLoading) return;

        StartCoroutine(LoadAsync(scene));
    }

    private IEnumerator LoadAsync(string scene)
    {
        _isLoading = true;

        _bgImage.enabled = true;

        float t = 0.0f;

        while(t < 1.0f)
        {
            t += Time.deltaTime / _fadeTime;
            _bgImage.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp(0.0f, 1.0f, t));
            yield return null;
        }

        _loadBarBG.enabled = true;
        _loadBarFill.enabled = true;
        _stateText.enabled = true;

        if (Random.Range(1, 100 + 1) <= 50)
        {
            _stateText.text = $"bienvenidos al el dia del payaso este dia los payasitos se reunen para hacer unos " +
                $"chistositos bien grasiositos \"oigale, cuente un chiste!\" \"....ehhhh....\" " +
                $"El dia del payaso 1 de abril de cualquier año, asi es, todos los payasitos se reunen para hacer " +
                $"felices a los niñitos, y a los grandecitos tambien- el dia del payaso 1 te comes 4 el dia del payaso" +
                $"Este dia, los niños se ponen felices, porque llega el payasito a darles unas graciosadas y unos" +
                $"chistositos \"Oye, esta el señor payaso, el señor payach-!\" ...." +
                $"El dia del payaso Hay muchos payasitos, esta el payasito chistosito, la payasita sin chichita," +
                $"el payacho cacho HAHAA el dia del payaso en otros lugares El dia del payaso se celebra en otro dia," +
                $"por ejemplo El Dia del payaso Se celebra en el dia del payaso el dia del payaso Todos los payasitos" +
                $"llegan en su carrito para .. Nose heh Subanse al carrito del payasito y disfruten sus payasadas en" +
                $"todo el camino \"Ya vamo a llegar?\" \"AGHAGHAGH\"\" Eh , eso no es un paya-\" El dia del payaso" +
                $"Ese dia usted vaya con el payasito y digale felicidades por El dia del payaso \"Hola soy un payaso\"" +
                $"\"HOLA SEÑOR PAYASO, FELICIDADES POR EL DIA DEL PAYASO\" lo golpea hasta que gime El dia, " +
                $"solo este dia se celebra en payasolandia, pero en otros lugares se celebra El dia del payaso " +
                $"El dia del payaso Muchos globitos, salen..c rie.. ???... to apreta- El dia del payaso Los payasitos" +
                $"tambien se encuentran en el internet, por ejemplo: el payaso libro y el ..." +
                $"el pwip HAHA el dia del payasoo \"SOY EL PAYACHITO, EL PAYACHITO DE PLAYA\" \"el playacho\" ...\"" +
                $"Oigan usted quien es?\" El dia del payaso Pero este solo se celebra pa' aya, aca somo El dia del " +
                $"payaso Asi que no esperes mas, celebra a tu payasito favorito este dia, y solo es 1, " +
                $"porque mañana es 2 HAHAAAAA, QUE TONTERIA EL DIA DEL PAYASO c rie mas El dia del payaso";
        }
        else
        {
            _stateText.text = $"Oh boy!";
        }

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scene);

        asyncOp.allowSceneActivation = false;

        while(asyncOp.progress < 0.9f)
        {
            _loadBarFill.fillAmount = asyncOp.progress / 0.9f;

            yield return null;
        }

        _loadBarBG.enabled = false;
        _loadBarFill.enabled = false;

        _stateText.text = "Press any key to continue.";

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        asyncOp.allowSceneActivation = true;

        _stateText.enabled = false;

        t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime / _fadeTime;
            _bgImage.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp(1.0f, 0.0f, t));
            yield return null;
        }

        _bgImage.enabled = false;

        _isLoading = false;
    }
}
