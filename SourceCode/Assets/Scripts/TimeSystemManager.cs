using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TimeSystemManager : MonoBehaviour
{
    public static Action OnDayChanged;
    public static Action OnMonthChanged;
    public static Action OnSeasonChanged;

    public static TimeSystemManager Instance{ get; private set; }

    public static int Day{ get; private set; }
    public static int Month{ get; private set; }
    public static int Season{ get; private set; }

    private float dayToRealTime = 10.0f;
    private float timer;

    private bool isPause;

    public TimeSystemManager(int initDay, int initMonth, int initSeason, float initDayToRealTime){
        Day = initDay;
        Month = initMonth;
        Season = initSeason;
        dayToRealTime = initDayToRealTime;
    }

    void Awake() {
        if( Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    void Start(){
        Day = 1;
        Month = 1;
        Season = 1;
        dayToRealTime = 10.0f;

        isPause = false;
    }

    void Update(){

        //如果玩家按了暂停 / 打开，则暂停时间
        if(Input.GetKeyDown(KeyCode.Escape)){
            isPause = !isPause;
            Debug.Log("isPause: " + isPause);
        }

        //In-game time 计时器
        if( !isPause ){
            timer -= Time.deltaTime;
        }
        
        if(timer <= 0){
            Day++;
            OnDayChanged?.Invoke();

            if(Day >= 30){
                Month++;

                Day = 1;
                OnMonthChanged?.Invoke();

                if(Month >= 1 && Month <= 3){
                    Season = 1;
                    OnSeasonChanged?.Invoke();
                }else if (Month > 3 && Month <= 6){
                    Season = 2;
                    OnSeasonChanged?.Invoke();
                }else if (Month > 6 && Month <= 9){
                    Season = 3;
                    OnSeasonChanged?.Invoke();
                }else if (Month > 9 && Month <= 12){
                    Season = 4;
                    OnSeasonChanged?.Invoke();
                }else{
                    Month = 1;
                    Season = 1;
                }
            }

            timer = dayToRealTime;
        }
    }

    //获取当前时间
    public InGameDate getCurrentTime(){
        return new InGameDate(Day, Month, Season);
    }

    //获取当前季节
    public int getSeason(){
        return Season;
    }

    //获取当前月份
    public int getMonth(){
        return Month;
    }
}

public class InGameDate{
    
    public int Day{ get; private set; }
    public int Month{ get; private set; }
    public int Season{ get; private set; }

    public InGameDate(int day, int month, int season){
        this.Day = day;
        this.Month = month;
        this.Season = season;
    }

    public override string ToString(){
        return base.ToString() + "Day: " + Day + " Month: " + Month + " Season " + Season; 
    }

}
