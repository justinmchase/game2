namespace Game {
  public class State {

    private static State _instance;
    public static State instance {get{
      if(State._instance == null){
        State._instance = new State();
      }

      return _instance;
    }}

    public int HP;
    

    public void InitState(){
      this.HP = 100;
    }
  }
}