//using System.Collections;
//using System.Collections.Generic;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.Networking;

//using ExcelDataReader;


public static class Funcs
{

	public static bool IsNull<T>(T script) where T : class
	{
		return script == null;
	}

	public static bool IsEqual<T>(T script1, T script2) where T : class
	{
		if (IsNull(script1) | IsNull(script2))
		{
			return false;
		}

		return script1.Equals(script2);
	}

	public static string GetEnumName<T>(int index) where T : struct, IConvertible
	{//where 조건 struct, IConvertible => Enum으로 제한
		return Enum.GetName(typeof(T), index);
	}

	
	public static int FlagToEnum(int flag)
	{
		if (flag <= 0)
		{
			return -1;
		}

		int temp = flag;
		int count = 0;
		while (true)
		{
			if (temp == 1)
			{
				break;
			}

			temp = temp >> 1;

			++count;
		}

		return count;
	}

	public static string TrimUnderBar(string str)
	{
		return str.Replace('_', ' '); 
	}

	public static string ExpandUnderBar(string str)
	{
		return str.Replace(' ', '_');
	}

	public static string SpacingByUpperCase(string str)
	{
		//첫글자는 신경안씀
		if (str == string.Empty || str.Length <= 1)
		{
			return str;
		}

		string resultStr = string.Empty;

		for (int i = 0; i < str.Length; ++i)
		{
			if (char.IsUpper(str[i]) && i != 0)
			{
				resultStr += ' ';
			}

			resultStr += str[i];
		}

		return resultStr;
	}


	public static int B2I(bool boolean)
    {
		//false => 값 무 (0)
		//true => 값 유 
        return Convert.ToInt32(boolean);
    }

	public static bool I2B(int integer)
	{
		return Convert.ToBoolean(integer);
	}

	public static bool IntegerRandomCheck(int percent)
	{
		int rand = UnityEngine.Random.Range(1, 101);

		if (rand > percent)
		{
			return false;
		}

		return true;
	}


	public static int DontOverlapRand(int curNum, int min, int ExclusiveMax)
	{
		int iRand;

		while (true)
		{
			iRand = UnityEngine.Random.Range(min, ExclusiveMax);

			if (iRand != curNum)
			{
				break;
			}
		}

		return iRand;
	}


	public static List<T> Shuffle<T>(List<T> list)
	{
		for (int i = list.Count - 1; i > 0; i--)
		{
			System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
			int rnd = random.Next(0, i);
			T temp = list[i];
			list[i] = list[rnd];
			list[rnd] = temp;
		}
		return list;
	}
	public static Vector3 Random(Vector3 min, Vector3 max)
	{
		float x = UnityEngine.Random.Range(min.x, max.x);
		float y = UnityEngine.Random.Range(min.y, max.y);
		float z = UnityEngine.Random.Range(min.z, max.z);

		return new Vector3(x, y, z);
	}

	public static Vector3 Vec3_Random(float min, float max)
	{
		float x = UnityEngine.Random.Range(min, max);
		float y = UnityEngine.Random.Range(min, max);
		float z = UnityEngine.Random.Range(min, max);

		return new Vector3(x, y, z);
	}

	//public static Structs.RayResult RayToWorld(Vector2 screenPos)
	//{
	//	//이걸 그냥 충돌한 놈이 그라운드 일때만 리턴하게?
	//	//아니면 소환하는 곳에서 충돌된 놈이 그라운드가 아니면 그 새기 크기 판단해서 옆에 생성되게?

	//	Structs.RayResult rayResult = new Structs.RayResult();

	//	Ray ray = Camera.main.ScreenPointToRay(screenPos);
	//	RaycastHit castHit;

	//	if (Physics.Raycast(ray, out castHit))
	//	{
	//		rayResult.hitPosition = castHit.point;
	//		rayResult.hitPosition.y = 0f;
	//		rayResult.hitObj = castHit.transform.gameObject;
	//		rayResult.isHit = true;
	//		rayResult.ray = ray;
	//		rayResult.rayHit = castHit;
	//	}
	//	else
	//	{
	//		rayResult.isHit = false;
	//	}

	//	return rayResult;
	//}

	public static void ChangeMesh(GameObject origin, Mesh mesh)
	{
		MeshFilter tempFilter = origin.GetComponent<MeshFilter>();

		if (tempFilter != null)
		{
			tempFilter.mesh = mesh;
		}
	}



	public static GameObject FindGameObjectInChildrenByName(GameObject Parent, string ObjName)
	{
		if (Parent == null)
		{
			return null;
		}

		//그냥 transform.Find 로 찾으면 한 단계 아래 자식들만 확인함.
		int childrenCount = Parent.transform.childCount;

		GameObject[] findObjs = new GameObject[childrenCount];

		if (Parent.name == ObjName)
		{
			return Parent;
		}

		if (childrenCount == 0)
		{
			return null;
		}
		else
		{
			for (int i = 0; i < childrenCount; ++i)
			{
				findObjs[i] = FindGameObjectInChildrenByName(Parent.transform.GetChild(i).gameObject, ObjName);

				if (findObjs[i] != null && findObjs[i].name == ObjName)
				{
					return findObjs[i];
				}
			}

			return null;
		}
	}

	public static GameObject FindGameObjectInChildrenByTag(GameObject Parent, string ObjTag)
	{
		if (Parent == null)
		{
			return null;
		}

		int childrenCount = Parent.transform.childCount;

		GameObject[] findObjs = new GameObject[childrenCount];

		if (Parent.CompareTag(ObjTag))
		{
			return Parent;
		}

		if (childrenCount == 0)
		{
			return null;
		}
		else
		{
			for (int i = 0; i < childrenCount; ++i)
			{
				findObjs[i] = FindGameObjectInChildrenByTag(Parent.transform.GetChild(i).gameObject, ObjTag);

				if (findObjs[i] != null && findObjs[i].CompareTag(ObjTag))
				{
					return findObjs[i];
				}
			}
			return null;
		}
	}
	public static T FindComponentInNearestParent<T>(Transform curTransform) where T : Component
	{
		if (curTransform == null)
		{
			return null;
		}

		T tempComponent = curTransform.GetComponent<T>();

		if (tempComponent == null)
		{
			if (curTransform.parent != null)
			{
				tempComponent = FindComponentInNearestParent<T>(curTransform.parent);
			}
			else
			{
				return null;
			}
		}

		return tempComponent;
	}


	public static void RagdollObjTransformSetting(Transform originObj, Transform ragdollObj)
	{
		for (int i = 0; i < originObj.childCount; ++i)
		{
			if (originObj.childCount != 0)
			{
				RagdollObjTransformSetting(originObj.GetChild(i), ragdollObj.GetChild(i));
			}

			ragdollObj.GetChild(i).localPosition = originObj.GetChild(i).localPosition;
			ragdollObj.GetChild(i).localRotation = originObj.GetChild(i).localRotation;
		}
	}

	

	public static bool IsAnimationAlmostFinish(Animator animCtrl, string animationName, float ratio = 0.95f )
	{
		if (animCtrl.GetCurrentAnimatorStateInfo(0).IsName(animationName))
		{//여기서 IsName은 애니메이션클립 이름이 아니라 애니메이터 안에 있는 노드이름임
			if (animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime >= ratio)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsAnimationCompletelyFinish(Animator animCtrl, string animationName, float ratio = 1.0f)
	{
		if (animCtrl.GetCurrentAnimatorStateInfo(0).IsName(animationName))
		{//여기서 IsName은 애니메이션클립 이름이 아니라 애니메이터 안에 있는 노드이름임
			if (animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime >= ratio)
			{
				return true;
			}
		}
		return false;
	}

	public static T FindResourceFile<T>(string path) where T : UnityEngine.Object
	{
		T temp = Resources.Load<T>(path);
		
		if (temp == null)
		{
			Debug.Log(path + "\nhas not exist!");
		}

		return temp;
	}

	public static GameObject CheckGameObjectExist(string name)
	{
		GameObject temp = GameObject.Find(name);

		if (temp == null)
		{
			temp = new GameObject(name);
		}

		return temp;
	}

	public static GameObject CheckGameObjectExist<T>(string objName) where T : Component
	{
		GameObject tempObj = GameObject.Find(objName);

		if (tempObj == null)
		{
			tempObj = new GameObject(objName);
		}

		T tempComponent = tempObj.GetComponent<T>();

		if (tempComponent == null)
		{
			tempObj.AddComponent<T>();
		}

		return tempObj;
	}

	public static T CheckComponentExist<T>(string gameObjectName) where T : Component
	{
		GameObject temp = GameObject.Find(gameObjectName);

		if (temp == null)
		{
			temp = new GameObject(gameObjectName);
			//temp.name = gameObjectName;
		}

		T tempComponent = temp.GetComponent<T>();

		if (tempComponent == null)
		{
			tempComponent = temp.AddComponent<T>();
		}

		return tempComponent;
	}

	/// <summary>
	/// 딱 한 계층의 부모, 자식만 체크하는거임
	/// 아예 전체 계층 구조에서 찾아야하면 재귀함수 불러 쓰삼
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj"></param>
	/// <returns></returns>
	public static T IsComponentExistInFamily<T>(GameObject obj) where T : Component
	{
		T temp = obj.GetComponent<T>();

		if (temp)
		{
			return temp;
		}
		else
		{
			temp = obj.GetComponentInParent<T>();

			if (temp)
			{
				return temp;
			}
			else
			{
				return obj.GetComponentInChildren<T>();
			}
		}
	}

	public static List<T> GetComponentInFamilly<T>(GameObject obj) where T : Component
	{
		List<T> list = new List<T>();

		T temp = obj.GetComponent<T>();
		if (temp)
		{
			list.Add(temp);
		}

		int childCoount = obj.transform.childCount;

		for (int i = 0; i < childCoount; ++i)
		{
			var list2 = GetComponentInFamilly<T>(obj.transform.GetChild(i).gameObject);

			foreach (T com in list2)
			{
				list.Add(com);
			}
		}

		return list;
	}

	public static Vector3 GetCenterPos(Vector3 _pos1, Vector3 _pos2)
	{
		Vector3 dir = (_pos1 - _pos2).normalized;
		float dist = Vector2.Distance(_pos1, _pos2);
		return _pos2 + (dir * (dist * 0.5f));
	}

	public static Vector2 GetCenterPos(Vector2 _pos1, Vector2 _pos2)
	{
		Vector2 dir = (_pos1 - _pos2).normalized;
		float dist = Vector2.Distance(_pos1, _pos2);
		return _pos2 + (dir * (dist * 0.5f));
	}


	public static string ReadTextFile(string filePath)
	{ //StreamingAssets/리소스내의 폴더 위치 적어주면 댐.
	  //유니티가 최종 실행파일을 빌드할때 Resources 내의 모든 파일들을 바이너리화 해서 뽑지 않음.
	  //사용되는것들(ex. 프리팹,모델,스크립트,Resources.load등으로 불러오는것만)만 뽑음
	  //StreamingAssets폴더내의 있는 파일들은 다 뽑아내기때문에
	  //유니티에서 지원하는?? 파일형식 제외하고는 저기 넣어야 안정적이겠즤

		//UnityEngine.Networking. UnityWebRequest 를 이용하여 안드로이드, PC모두 읽어올 수 있도록
		string androidPath = Application.streamingAssetsPath + "/" + filePath + ".txt";

		UnityWebRequest uwrFile = UnityWebRequest.Get(androidPath);
		uwrFile.SendWebRequest();
		while (!uwrFile.isDone)
		{
			//파일 읽어올때까지 대기하기 이거 비동기방식임.	
		}
		string str = "";
		// str = uwrFile.downloadHandler.text;
		if (uwrFile.result == UnityWebRequest.Result.Success)
		{
			str = uwrFile.downloadHandler.text;
		}
		else/*(uwrFile.result == UnityWebRequest.Result.ProtocolError)*/
		{
			Debug.Log(filePath + "에는 파일이 없읍니다. 확인요망");
			str = "TextFile has no exist";
		}
		return str;
	}


	//public static List<string> ExcelFileReader_Sheet(string filePath)
	//{
	//	List<string> resultStr = new List<string>();

	//	//string loadString = string.Empty;
	//	string fullPath = Application.streamingAssetsPath + "/" + filePath + ".xlsx";
	//	var stream = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
	//	var reader = ExcelReaderFactory.CreateReader(stream);

	//	var result = reader.AsDataSet();

	//	for (int sheet = 0; sheet < result.Tables.Count; ++sheet)
	//	{
	//		string sheetStr = string.Empty;

	//		for (int row = 1; row < result.Tables[sheet].Rows.Count; ++row)
	//		{
	//			int col = 0;
	//			while (true)
	//			{
	//				if (col >= result.Tables[sheet].Rows[row].ItemArray.Length)
	//				{//아마 rows의 length는 가장 긴 열 기준으로 자동으로 잡는거 같음.
	//					sheetStr += '\n';
	//					break;
	//				}
	//				string cellStr = result.Tables[sheet].Rows[row][col].ToString();
	//				if (string.Compare(cellStr, string.Empty) == 0)
	//				{
	//				//loadString += cellStr;
	//					sheetStr += '\n';
	//					break;
	//				}
	//				else
	//				{
	//					if (col != 0)
	//					{
	//						cellStr = ',' + cellStr;
	//					}
	//					sheetStr += cellStr;
	//				}
	//				++col;
 //               }
 //           }
	//		resultStr.Add(sheetStr);
	//	}

	//	return resultStr;
	//}
	
	/// <summary>
	/// Return Value's Key is sheetName, Val is strings at sheet
	/// </summary>
	/// <param name="filePath"></param>
	/// <returns></returns>
	//public static Dictionary<string,string> ExcelFileReader(string filePath)
	//{
	//	Dictionary<string, string> resultDic = new Dictionary<string, string>();
	//	//List<string> resultStr = new List<string>();

	//	//string loadString = string.Empty;
	//	string fullPath = Application.streamingAssetsPath + "/" + filePath + ".xlsx";
	//	var stream = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
	//	var reader = ExcelReaderFactory.CreateReader(stream);

	//	var result = reader.AsDataSet();

	//	for (int sheet = 1; sheet < result.Tables.Count; ++sheet)
	//	{
	//		string sheetStr = string.Empty;

	//		for (int row = 1; row < result.Tables[sheet].Rows.Count; ++row)
	//		{
	//			int col = 0;
	//			while (true)
	//			{
	//				if (col >= result.Tables[sheet].Rows[row].ItemArray.Length)
	//				{//아마 rows의 length는 가장 긴 열 기준으로 자동으로 잡는거 같음.
	//					sheetStr += '\n';
	//					break;
	//				}
	//				string cellStr = result.Tables[sheet].Rows[row][col].ToString();
	//				if (string.Compare(cellStr, string.Empty) == 0)
	//				{
	//					//loadString += cellStr;
	//					sheetStr += '\n';
	//					break;
	//				}
	//				else
	//				{
	//					if (col != 0)
	//					{
	//						cellStr = ',' + cellStr;
	//					}
	//					sheetStr += cellStr;
	//				}
	//				++col;
	//			}
	//		}
	//		//resultStr.Add(sheetStr);
	//		resultDic.Add(result.Tables[sheet].TableName, sheetStr);
	//	}

	//	return resultDic;
	//}


	//public static string ExcelFileReader(string filePath, int sheetIndex)
	//{
	//	string loadString = string.Empty;
	//	string fullPath = Application.streamingAssetsPath + "/" + filePath + ".xlsx";
	//	var stream = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
	//	var reader = ExcelReaderFactory.CreateReader(stream);

	//	var result = reader.AsDataSet();

 //       for (int row = 1; row < result.Tables[sheetIndex].Rows.Count; ++row)
 //       {
 //           int col = 0;
 //           while (true)
 //           {
 //               if (col >= result.Tables[sheetIndex].Rows[row].ItemArray.Length)
 //               {//아마 rows의 length는 가장 긴 열 기준으로 자동으로 잡는거 같음.
 //                   loadString += '\n';
 //                   break;
 //               }
 //               string cellStr = result.Tables[sheetIndex].Rows[row][col].ToString();
 //               if (string.Compare(cellStr, string.Empty) == 0)
 //               {
 //                   loadString += '\n';
 //                   break;
 //               }
 //               else
 //               {
 //                   if (col != 0)
 //                   {
 //                       cellStr = ',' + cellStr;
 //                   }
 //                   loadString += cellStr;
 //               }
 //               ++col;
 //           }
 //       }

 //       return loadString;
	//}


/// <summary>
/// return string is divided string line.
/// </summary>
/// <param name="origin"></param>
/// <returns></returns>
	public static string DivideLine(ref string origin)
	{
		if (!origin.Contains('\n'))
		{
			Debug.LogWarning("줄 나눔 기호 없는디 ㅋ;");
			return string.Empty;
		}

		string originCopy = origin;		
		string temp = string.Empty;

        for (int i = 0; i < origin.Length; ++i)
        {
            if (origin[i] == '\n')
            {
				originCopy = originCopy.Remove(0, 1);
				break;
            }
            else
            {
                temp += origin[i];
				originCopy = originCopy.Remove(0,1);
            }
        }

		origin = originCopy;
		return temp;
        //var strings = origin.Split('\n');
    }

	public static string RemoveTextInParentheses(string origin)
	{
		int startIndex = origin.IndexOf('(');
		int endIndex = origin.IndexOf(')');

		if (startIndex == -1 || endIndex == -1)
		{
			//Debug.LogError("괄호 시작~끝 중에 하나는 없다 슈`벌럼아~");
			return origin;
		}

		return origin.Remove(startIndex, endIndex - startIndex + 1);
	}



	//public static void TESTFUNC(List<string> val, ref Statistic stat)
 //   {

	//	//1. string 형
	//	//2. 정수형
	//	//3. 실수형
	//	//4. Vector2형
	//	//5. Resource 형
	//	int tempInt;
	//	float tempFloat;
		
	//	if (val.Count == 6)
	//	{ //Resource형
				

			
	//	}
	//	else if (val.Count > 2)
	//	{ //Vector2 형

	//	}
	//	else if (int.TryParse(val[1], out tempInt))
	//	{

	//	}
	//	else if (float.TryParse(val[1], out tempFloat))
	//	{

	//	}
	//	else
	//	{ //이러면 걍 string ㅋㅋ
		
	//	}

 //   }

 //   public static KeyValuePair<string, List<string>> DivideComma(string origin)
	//{
	//	if (!origin.Contains(','))
	//	{
	//		Debug.LogWarning("콤마 없는디 ㅋ;");
	//		return new KeyValuePair<string, List<string>>();
	//	}

	//	var resultStr = origin.Split(',');

	//	List<string> vals = new List<string>();

	//	for (int i = 1; i < resultStr.Length; ++i)
	//	{
	//		vals.Add(resultStr[i]);
	//	}
		
	//	return new KeyValuePair<string, List<string>>(resultStr[0], vals);
	//}

	public static List<string> DivideComma(string origin)
	{
		if (!origin.Contains(','))
		{
			Debug.LogWarning("콤마 없는디 ㅋ;");
			return null;
		}

		var resultStr = origin.Split(',');

		return resultStr.ToList();
	}

	//public static void ParsingObjectStat(List<string> list, Statistic stat)
	//{ 
	
	//}


	public static void LineToList(string origin, ref List<string> list)
	{
		string line = string.Empty;

		for (int i = 0; i < origin.Length; ++i)
		{
			if (origin[i] == '\n')
			{
				list.Add(line);
				line = string.Empty;
			}
			else
			{
				line += origin[i];
			}
		}
	}



	public static Color SetAlpha(Color color, float alpha)
	{
		Color tempColor = color;
		tempColor.a = alpha;
		return tempColor;
	}

	public static void SetAlpha(ref Color color, float alpha)
	{
		Color tempColor = color;
		tempColor.a = alpha;
		color = tempColor;
	}

	public static void SetColor(ref Color refVal, Color color, float alpha = 1f)
	{
		refVal = color;
		refVal.a = alpha;
	}

	public static void SetGizmoColor(Color color, float alpha = 1f)
	{
		Color temp = color;
		temp.a = alpha;
		Gizmos.color = temp;
	}

	//public static bool IsOnlyTypeInList<T>(List<CObj> objs) where T : CObj
	//{
	//	var tempList = objs.ToList();

	//	tempList.RemoveAll(x => x is T);

	//	return tempList.Count == 0;
	//}


	//	public static Vector2 UnityToWindow(Vector2 unityScreen)
	//	{
	//		var appPos = Screen.mainWindowPosition;
	//		var appRes = new Vector2(Screen.width, Screen.height);

	//		//여기서 에디터일경우
	//		//에디터의 게임뷰의 스케일이 얼마인지
	//		//+ 실제 렌더링 범위와 에디터 게임 뷰의 상단 탭 크기
	//		//약 50으로 추정
	//		//다 고려해야함... ;;;

	//#if UNITY_EDITOR
	//		float scale = UnityEditor.Handles.GetMainGameViewSize().x / Screen.width;
	//#endif


	//		return new Vector2(unityScreen.x + appPos.x,  appRes.y - unityScreen.y + appPos.y);
	//	}

	//public static Vector2 WindowToUnity(Vector2 windowScreen)
	//{
	//	var appPos = Screen.mainWindowPosition;
	//	var appRes = new Vector2(Screen.width, Screen.height);

	//	return new Vector2(windowScreen.x - appPos.x, windowScreen.y - appPos.y - appRes.y);
	//}


	public static T DeepCopy<T>(this T source) where T : new()
	{
		if (!typeof(T).IsSerializable)
		{
			Debug.Log(nameof(T) + " 클래스는 직렬화가 안되어 딮카피 실패");
			return source;
		}

		//try
		//{
			object result = null;
			//using문 -> IDisposable 객체들의 올바른 사용을 보장하는 키워드
			//DisPose 자체가 '해제;임
			//즉 메모리 해제를 개발자가 해줘야하는 경우
			//해당 using 키워드를 사용하면 알아서 다 쓰면 지워줌 ㅋ
			using (var ms = new MemoryStream()) 
			{
				var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				formatter.Serialize(ms, source);
				ms.Position = 0;
				result = (T)formatter.Deserialize(ms);
				ms.Close();
			}

			return (T)result;
		//}
		//catch (Exception)
		//{
		//	Debug.Log(nameof(T) + " 클래스 딮카피 실패");
		//	return new T();
		//}
	}

	//public static string ShowSaveFileDialog()
	//{
	//	//여기서는 그냥 폴더 주소만 찾아주는 역할.
	//	//실제 세이브/로드는 다른 함수에서!

	//	System.Windows.Forms.SaveFileDialog dia = new System.Windows.Forms.SaveFileDialog();

	//	string path = UnityEngine.Application.dataPath;//프로젝트 폴더내의 Asset폴더
	//												   //Debug.Log(path); 
	//	dia.InitialDirectory = path + "../"; //Dialog창 띄울때 열릴 폴더 위치
	//	dia.Title = "Open Map Data File"; //
	//	dia.DefaultExt = "bin";
	//	dia.Filter = "모든 파일 (*.*) | *.*";

	//	var diaResult = dia.ShowDialog();

	//	if (diaResult == System.Windows.Forms.DialogResult.OK)
	//	{//okay 버튼 눌렀을때

	//		//File경로와 File명을 모두 가지고 온다.
	//		string temp = dia.FileName;

	//		return dia.FileName;
	//	}

	//	return string.Empty;
	//}

	//public static string ShowOpenFileDialog()
	//{
	//	System.Windows.Forms.OpenFileDialog dia = new System.Windows.Forms.OpenFileDialog();

	//	string path = UnityEngine.Application.dataPath;//프로젝트 폴더내의 Asset폴더
	//												   //Debug.Log(path); 
	//	dia.InitialDirectory = path + "../"; //Dialog창 띄울때 열릴 폴더 위치
	//	dia.Title = "Test Dialog Window"; //
	//									  //dia.DefaultExt = "bin";
	//	dia.Filter = "binaryFile (*.bin)|*.bin";

	//	var diaResult = dia.ShowDialog();

	//	if (diaResult == System.Windows.Forms.DialogResult.OK)
	//	{//okay 버튼 눌렀을때

	//		//File경로와 File명을 모두 가지고 온다.
	//		string temp = dia.FileName;
	//		return temp;
	//	}

	//	return string.Empty;
	//}

	public static T LoadBinary<T>(string path)
	{
		if (path.Equals(string.Empty))
		{
			return default(T);
		}

		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Open(path, FileMode.Open);

		if (fileStream != null && fileStream.Length > 0)
		{
			T loadTemp = (T)binaryFormatter.Deserialize(fileStream);

			return loadTemp;
		}
		else
		{
			Debug.Log("파일 이상한디");

			return default(T);
		}
	}





	


}

public static class Defines
{
	public const int right = 1;
	public const int left = 0;

	public const int ally = 0;
	public const int enemy = 1;

	public const float winCX = 1600f;
	public const float winCY = 900f;

	public const int tileX = 256;
	public const int tileY = 256;
	public const int tileCount = 256;

	public const int barrackQueueCount = 5;

	public const float gravity = -9.8f;

	public const float PI = 3.14159265f;


	//D_Days[Difficulty, raidCount]
	public static int[,] D_Days =	{
										{ 15, 25, 30, 35, 40},
										{ 15, 30, 40, 50, 65},
										{ 1, 35, 55, 75, 90}
									};
	//top, right, bottom, left
	public static Vector2Int[] RaidSpawnIndex = {
													new Vector2Int(0,0),
													new Vector2Int(tileCount,0),
													new Vector2Int(tileCount,tileCount),
													new Vector2Int(0,tileCount),
												};


	public static string managerPrfabFolderPath = "Prefabs/Managers/";
	public static bool DESTROY = false;
	public static bool DONT_DESTROY = true;
	
	
	public static string MapDataFileName = "MapData.bin";
	public static string ObjStatFileName = "ObjectStat";

	

	public const float minimapSize = 256;


	public static Vector2[] ViewportPos =
	{
		new Vector2(0,1),	//LT
		new Vector2(1,1),	//RT
		new Vector2(1,0),	//RB
		new Vector2(0,0),//LB
		new Vector2(0.5f, 0.5f) //Center
	};

}

namespace Enums
{
	public enum eScenes
	{ 
		Intro,
		Title,
		Game,
		End
	}


	
	public enum eTargetPriority
	{ 
		Front,
		Rear,
		Close,
		Random,
		MaxHpAmount,
		MinHpAmount,
		CurHp,
		End
	}

	public enum eShooter0Status
	{
		Idle,
		Attack,
		Death,
		End
	}

    public enum eShooter1Status
    {
        Idle,
        Trace,
        Attack,
        Death,
		End
    }
}

namespace Structs
{

	[System.Serializable]
	public struct Boundary
	{
		public Boundary(float _xMin, float _xMax, float _yMin, float _yMax)
		{
			xMin = _xMin;
			xMax = _xMax;

			yMin = _yMin;
			yMax = _yMax;

			yMin = _yMin < _yMax ? _yMin : _yMax;
			yMax = _yMin < _yMax ? _yMax : _yMin;

		}

		public Boundary(Vector2 center, Vector2 size, bool isCenterCalc)
		{
			xMin = center.x - (size.x * 0.5f);
			xMax = center.x + (size.x * 0.5f);

			yMin = center.y - (size.y * 0.5f);
			yMax = center.y + (size.y * 0.5f);
		}

		public Boundary(Vector2 _LT, Vector2 _RB)
		{
			xMin = _LT.x;
			xMax = _RB.x;

			yMin = _LT.y < _RB.y ? _LT.y : _RB.y;
			yMax = _LT.y > _RB.y ? _LT.y : _RB.y;
		}

		public static Boundary zero
		{
			get
			{
				var bound = new Boundary();
				bound.xMin = 0f;
				bound.xMax = 0f;
				bound.yMin = 0f;
				bound.yMax = 0f;

				return bound;
			}
		}

		public Vector2 length
		{
			get 
			{ 
				return new Vector2(Mathf.Abs(this.xMax - this.xMin), Mathf.Abs(this.yMax - this.yMin));
			}
		}

		public bool IsInside_Include(Vector2 pos)
		{

			if (pos.x <= xMax & pos.x >= xMin &
				pos.y <= yMax & pos.y >= xMin)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool IsInside_Exclude(Vector2 pos)
		{

			if (pos.x  < xMax & pos.x > xMin &
				pos.y < yMax & pos.y > xMin)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		public float xMin;
		public float xMax;

		public float yMin;
		public float yMax;
	}

	//[System.Serializable]
	//public struct RayResult
	//{
	//	//public RayResult()
	//	//{
	//	//	isHit = false;
	//	//	hitPosition = Vector3.negativeInfinity;
	//	//	objectScript = null;
	//	//}

	//	public bool isHit;
	//	public Vector3 hitPosition;
	//	public CObj objectScript;
		
	//	public RaycastHit rayHit;
	//}
	
	//[System.Serializable]
	//public struct DmgStruct
	//{
	//	public DmgStruct(CObj _body, int _dmg, bool _isCrit)
	//	{
	//		atkObj = _body;
	//		dmg = _dmg;
	//		isCrit = _isCrit;
	//	}

	//	public DmgStruct(CObj _body, int _dmg)
	//	{
	//		atkObj = _body;
	//		dmg = _dmg;
	//		isCrit = false;
	//	}

	//	public DmgStruct(CObj _body, float _dmg)
	//	{
	//		atkObj = _body;
	//		dmg = (int)_dmg;
	//		isCrit = false;
	//	}

	//	public CObj atkObj;
	//	public int dmg;
	//	public bool isCrit;
	//}
}

namespace UI
{
	public enum FADE
	{ 
		IN,
		OUT
	}
}


namespace Johnson
{
	public enum TIME_SCALE
	{ 
		PAUSE = 0,
		X1 = 1,
		X2 = 2,
		X4 = 4,
		X10 = 10
	}

	



}