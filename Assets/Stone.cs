using UnityEngine;
using Reversi;

public class Stone : MonoBehaviour
{
    [SerializeField]
    private GameObject stoneObject;

    private Animator _animator;

    [SerializeField]
    private StoneState _state;


    public StoneIndex Index { get; private set; }


    public StoneState StoneState
    {
        get => _state;
        set
        {
            StateChangedAnim(_state, value);
            _state = value;
        }
    }
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
   
    private void Awake()
    {
        stoneObject.SetActive(false);
    }


    private void Update()
    {
       
    }

    private void StateChangedAnim(StoneState prev, StoneState value)
   {
       
        if (prev == value) { return; }
        switch (value)
        {
            // Emptyの場合は非表示
            case StoneState.Empty:
                stoneObject.SetActive(false);
                break;
            // 色が指定されたら表示
            case StoneState.White:
                stoneObject.SetActive(true);
                if (prev == StoneState.Black)
                {
                    // _animator.Play("BWAnimation");
                  
                    transform.Rotate(180.0f, 0.0f, 0.0f);
                }
                break;
            case StoneState.Black:
                stoneObject.SetActive(true);
                if (prev == StoneState.Empty) transform.Rotate(180.0f, 0.0f, 0.0f);
                if (prev == StoneState.White)
                {
                    // _animator.Play("WBAnimation");
                  
                    transform.Rotate(180.0f, 0.0f, 0.0f);

                }
                break;
        }
    }
}




