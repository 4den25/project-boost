using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField][Range(0, 1)] float movementFactor;
    [SerializeField] float period = 2f;
    //period is the amount of time before the sin wave repeats itself,
    //we can change it to make the object oscillate faster or slower
    //trong cùng một Time.time, period càng ngắn thì số vòng mà đồ thị hình sin lặp lại càng nhiều
    //ví dụ 1: Time.time = 2, period = 2f, số vòng sin wave đã lặp sẽ = 1
    //trong 2s, object sẽ dao động từ điểm đầu -> điểm cuối và ngược lại 1 vòng trong 2s
    //ví dụ 2: Time.time = 2, period = 0.5f, số vòng sin wave đã lặp sẽ = 4
    //trong 2s, object sẽ dao động từ điểm đầu -> điểm cuối và ngược lại 4 vòng trong 2s (much faster)
    //cách giải thích trên là để áp vào thuật toán trong method update luôn
    //còn giải thích đơn giản thì object dao động qua lại theo dao động của sin wave
    //thời gian sin wave dao động xong 1 vòng (period) = thời gian object dao động qua lại xong 1 vòng
    //cho nên period càng ít thì thời gian object dao động qua lại xong 1 vòng càng ít (faster)

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position; //get the current position of the object
    }

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon) { return; } //it is safer than period == 0f

        float cycles = Time.time / period; //Time.time is the amount of time having elapsed

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);
        // we break the amount of time having elapsed into cycles,
        // angle of a whole cycle = tau radian (2pi),
        // by multing cycles (the time having elapsed) with tau, 
        // we got the total angle of cycles having gone within the elapse time
        // the more time passed, the more that total angel increase
        // but the rawSinWave always oscillate in the range [-1,1]

        movementFactor = (rawSinWave + 1f) / 2f; //rawSinWave goes from -1 -> 1, so we have to change it to 0 -> 1

        Vector3 offset = movementVector * movementFactor; 
        //movementVector là Vector từ điểm bắt đầu của object đến điểm cuối mà object dịch chuyển đến
        //offset là phân đoạn của movementVector, phải cộng điểm bắt đầu của object với offset dao động
        //liên tục theo thời gian để object có thể di chuyển từ từ đến điểm cuối và ngược lại
        //nếu chỉ cộng thẳng điểm bắt đầu với movementVector thì ocject chỉ có thể nhảy thẳng đến điểm cuối
        //chứ không dao động được

        transform.position = startingPosition + offset;
    }
}
