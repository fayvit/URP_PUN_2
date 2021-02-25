using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DragSupport : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Image bgImage;
    [SerializeField] private Image joystickKnobImage;
    [SerializeField] private float distanciaDeAfastamentoDoJoystick = 4;
    [SerializeField] private bool joystickComPosicaoFixa;
    [SerializeField] private float percenteStartScreenX = 0;
    [SerializeField] private float percenteStartScreenY = 0;
    [SerializeField] private float percenteEndScreenX = 0.5f;
    [SerializeField] private float percenteEndScreenY = 1;

    private int fId = -1;
    private Vector3 inputVector;
    private Vector3[] fourCornersArray = new Vector3[4];
    private Vector2 bgImageStartPosition;


    public Vector3 InputVector{ get=>inputVector; private set =>inputVector=value; }

    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(DragSupport))]
    public class InteractiveUiBaseEditor : Editor
    {
        SerializedProperty sBgImage;
        SerializedProperty sjoystickKnobImage;
        SerializedProperty sAfast;
        SerializedProperty sPosFixa;
        SerializedProperty sStartPercX;
        SerializedProperty sStartPercY;
        SerializedProperty sEndPercX;
        SerializedProperty sEndPercY;

        void OnEnable()
        {
            sBgImage = serializedObject.FindProperty("bgImage");
            sjoystickKnobImage = serializedObject.FindProperty("joystickKnobImage");
            sAfast = serializedObject.FindProperty("distanciaDeAfastamentoDoJoystick");
            sPosFixa = serializedObject.FindProperty("joystickComPosicaoFixa");
            sStartPercX = serializedObject.FindProperty("percenteStartScreenX");
            sStartPercY = serializedObject.FindProperty("percenteStartScreenY");
            sEndPercX = serializedObject.FindProperty("percenteEndScreenX");
            sEndPercY = serializedObject.FindProperty("percenteEndScreenY");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            DragSupport d = target as DragSupport;
            SerializedProperty prop = serializedObject.FindProperty("m_Script");
            EditorGUILayout.PropertyField(prop, false, new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(sBgImage, true, new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(sjoystickKnobImage, true, new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(sAfast, true, new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(sPosFixa, true, new GUILayoutOption[0]);

            

            if (!d.joystickComPosicaoFixa)
            {
                EditorGUILayout.PropertyField(sStartPercX, true, new GUILayoutOption[0]);
                EditorGUILayout.PropertyField(sStartPercY, true, new GUILayoutOption[0]);
                EditorGUILayout.PropertyField(sEndPercX, true, new GUILayoutOption[0]);
                EditorGUILayout.PropertyField(sEndPercY, true, new GUILayoutOption[0]);
            }

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.EndVertical();
            
        }

        
    }
#endif
    #endregion

    public void Start()
    {
       // bgImage = GetComponent<Image>();
       // joystickKnobImage = transform.GetChild(0).GetComponent<Image>();

        bgImage.rectTransform.GetWorldCorners(fourCornersArray);

        bgImageStartPosition = fourCornersArray[3];
        bgImage.rectTransform.pivot = new Vector2(1, 0);

        bgImage.rectTransform.anchorMin = new Vector2(0, 0);
        bgImage.rectTransform.anchorMax = new Vector2(0, 0);
        bgImage.rectTransform.position = bgImageStartPosition;

        if (!joystickComPosicaoFixa)
        {
            SetView(false);
        }
    }

    void SetView(bool b)
    {
        bgImage.enabled = b;
        joystickKnobImage.enabled = b;
    }

    void SetPos(Vector2 pos)
    {
        
        Rect rect = bgImage.rectTransform.rect;
        Vector2 varV = new Vector2(rect.width/2, -rect.height/2);
            
        bgImage.rectTransform.anchoredPosition = pos + varV;

        
    }

    private void Update()
    {
        if (!joystickComPosicaoFixa)
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch T = Input.GetTouch(i);
                if (!bgImage.enabled)
                {
                    if (T.phase == TouchPhase.Began)
                    {
                        if (T.position.x > percenteStartScreenX * Screen.width
                            && T.position.x < percenteEndScreenX * Screen.width
                            && T.position.y > percenteStartScreenY * Screen.height
                            && T.position.y < percenteEndScreenY * Screen.height
                            )
                        {

                            SetPos(T.position);
                            SetView(true);
                            fId = T.fingerId;
                            bgImageStartPosition = T.position;

                            PointerEventData __newPointerEventData = new PointerEventData(null);
                            __newPointerEventData.position = T.position;


                            ExecuteEvents.Execute(gameObject, __newPointerEventData, ExecuteEvents.beginDragHandler);

                            //  antPos = T.position;
                        }
                    }
                }
                else
                {
                    if (T.phase == TouchPhase.Ended && T.fingerId == fId)
                    {
                        SetView(false);
                        OnPointerUp(null);
                        fId = -1;
                    }
                    else if ((T.phase == TouchPhase.Moved || T.phase == TouchPhase.Stationary) && T.fingerId == fId)
                    {
                        PointerEventData __newPointerEventData = new PointerEventData(null);
                        __newPointerEventData.position = T.position;
                        // __newPointerEventData.delta = T.position - antPos;

                        ExecuteEvents.Execute(gameObject, __newPointerEventData, ExecuteEvents.dragHandler);
                        //antPos = T.position;
                    }


                }
            }
    }

    public void MoveDrag(Vector2 localPoint)//,Vector2 ped)
    {
        localPoint.x = (localPoint.x / bgImage.rectTransform.sizeDelta.x);
        localPoint.y = (localPoint.y / bgImage.rectTransform.sizeDelta.y);

        inputVector = new Vector3(localPoint.x * 2 + 1, localPoint.y * 2 - 1, 0);

        //Vector3 unNormalizedInput = inputVector;// para deslocamento do joystick suprimido

        inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

        joystickKnobImage.rectTransform.anchoredPosition =
         new Vector3(inputVector.x * (bgImage.rectTransform.sizeDelta.x / distanciaDeAfastamentoDoJoystick),
                     inputVector.y * (bgImage.rectTransform.sizeDelta.y / distanciaDeAfastamentoDoJoystick));

        #region deslocamentoDoJoystickSuprimido
        //if (!joystickComPosicaoFixa)
        //{
        //    // if dragging outside the circle of the background image
        //    if (unNormalizedInput.magnitude > inputVector.magnitude)
        //    {
        //        var currentPosition = bgImage.rectTransform.position;
        //        currentPosition.x += ped.x;
        //        currentPosition.y += ped.y;

        //        // keeps the joystick on the left-hand half of the screen
        //        currentPosition.x = Mathf.Clamp(currentPosition.x, percenteStartScreenX * Screen.width
        //            + bgImage.rectTransform.sizeDelta.x, Screen.width * percenteEndScreenX);
        //        currentPosition.y = Mathf.Clamp(currentPosition.y, percenteStartScreenY * Screen.height,
        //            percenteEndScreenY * Screen.height - bgImage.rectTransform.sizeDelta.y);

        //        // moves the entire joystick along with the drag  
        //        bgImage.rectTransform.position = currentPosition;
        //    }
        //}
        #endregion
    }

    public void OnDrag(PointerEventData ped)
    {
        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, ped.position, ped.pressEventCamera, out localPoint))
        {
            MoveDrag(localPoint);//, ped.delta);
        }
    }

    public void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputVector = Vector3.zero;
        joystickKnobImage.rectTransform.anchoredPosition = Vector3.zero;
    }
}
