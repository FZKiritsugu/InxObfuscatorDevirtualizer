
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
public class Inx : Inx2
{
	[Obfuscation(Exclude = false, StripAfterObfuscation = true)]
	public static DynamicMethod Execute(object[] parameters, MethodBase method, int one, int two, int three)
	{
		int key = two;
		if (VM.cache.TryGetValue(key, out VM.value))
		{
			return (DynamicMethod)VM.value.Invoke(null, parameters);
		}
		byte[] array = VM.byteArrayGrabber(Class.byteArrayResource, three, one);
		byte[] data = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(method.Name));
		byte[] iLAsByteArray = method.GetMethodBody().GetILAsByteArray();
		Class.bc(new Cryptographer("أ\u064b").Encrypt(array), new Cryptographer("أ\u064b").Encrypt(array).Length, new Cryptographer("أ\u064b").Encrypt(iLAsByteArray), new Cryptographer("أ\u064b").Encrypt(iLAsByteArray).Length);
		byte[] data2 = VM.Decrypt(VM.eBytes.Decrypt(data), array);
		int iD = two;
		return Inx2.___(method, parameters, iD, new Cryptographer("أ\u064b").Encrypt(data2));
	}
}
