
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InxObfuscatorDevirtualizer
{
    internal class Program
    {
        public static Assembly asm;
        public static ModuleDefMD module;
        static void Main(string[] args)
        {
            Console.Title = "InxObfuscatorDevirter - by 0x29A";
            asm = Assembly.UnsafeLoadFrom(args[0]);
            module = ModuleDefMD.Load(args[0]);
            Class.Init(FindInitialiseResourceName());
            foreach (var t in module.Types)
            {
                foreach (var m in t.Methods)
                {
                    if (!m.HasBody) continue;
                    for (int i = 0; i < m.Body.Instructions.Count; i++)
                    {
                       if (m.Body.Instructions[i].OpCode == OpCodes.Call &&
                                     m.Body.Instructions[i].Operand.ToString().Contains("Inx::Execute") && m.Body.Instructions[i - 1].OpCode == OpCodes.Call && m.Body.Instructions[i - 2].OpCode == OpCodes.Ldstr && m.Body.Instructions[i - 3].OpCode == OpCodes.Ldstr && m.Body.Instructions[i - 4].OpCode == OpCodes.Call && m.Body.Instructions[i - 5].OpCode == OpCodes.Ldstr && m.Body.Instructions[i - 6].OpCode == OpCodes.Ldstr && m.Body.Instructions[i - 7].OpCode == OpCodes.Call && m.Body.Instructions[i - 8].OpCode == OpCodes.Ldstr && m.Body.Instructions[i - 9].OpCode == OpCodes.Ldstr)
                            {
                                Console.WriteLine($"Devirtualized: {m.FullName}");
                              
                            var nigger = m.Body.Instructions[i - 2].Operand.ToString();
                            var nigger2 = m.Body.Instructions[i - 3].Operand.ToString();
                            var nigger3 = m.Body.Instructions[i - 5].Operand.ToString();
                            var nigger4 = m.Body.Instructions[i - 6].Operand.ToString();
                            var nigger5 = m.Body.Instructions[i - 8].Operand.ToString();
                            var nigger6 = m.Body.Instructions[i - 9].Operand.ToString();
                       
                            int one = Convert.ToInt32(Xoring(nigger2, nigger));
                            int two = Convert.ToInt32(Xoring(nigger4, nigger3));
                            int three = Convert.ToInt32(Xoring(nigger6, nigger5));

                            object[] Params = new object[m.Parameters.Count]; int Index = 0;
                            foreach (var Param in m.Parameters) { Params[Index++] = Param.Type.Next; }
                            var methodBase = asm.ManifestModule.ResolveMethod(m.MDToken.ToInt32());
                            var dynamicMethod = Inx.Execute(Params, methodBase, one, two, three);
                            var dynamicReader = Activator.CreateInstance(
                                               typeof(System.Reflection.Emit.DynamicMethod).Module.GetTypes()
                                                .FirstOrDefault(tm => tm.Name == "DynamicResolver"),
                                                (System.Reflection.BindingFlags)(-1), null, new object[] { dynamicMethod.GetILGenerator() }, null);
                            var dynamicMethodBodyReader = new DynamicMethodBodyReader(m.Module, dynamicReader);
                            dynamicMethodBodyReader.Read();
                            m.Body = dynamicMethodBodyReader.GetMethod().Body;

                        } 
                    }
                }
            }
 
            module.Write("devirtualized.exe", new ModuleWriterOptions(module)
            {
                MetadataOptions = { Flags = MetadataFlags.PreserveAll },
                MetadataLogger = DummyLogger.NoThrowInstance
            });
            Console.ReadKey();
        }


        public unsafe static string Xoring(string P_0, string P_1)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(P_0);
            byte[] bytes2 = Encoding.UTF8.GetBytes(P_1);
            fixed (byte* ptr = bytes)
            {
                fixed (byte* ptr3 = bytes2)
                {
                    byte* ptr2 = ptr;
                    byte* ptr4 = ptr3;
                    int num = Math.Min(bytes.Length, bytes2.Length);
                    for (int i = 0; i < num; i++)
                    {
                        byte* intPtr = ptr2++;
                        *intPtr = (byte)(*intPtr ^ *(ptr4++));
                    }
                }
            }
            return Encoding.UTF8.GetString(bytes);
        }


        public static string FindInitialiseResourceName()
        {
            foreach (var t in module.Types)
            {
                foreach (var m in t.Methods)
                {
                    if (!m.HasBody) continue;
                    for (int i = 0; i < m.Body.Instructions.Count; i++)
                    {
                        if (m.Body.Instructions[i].OpCode == OpCodes.Call &&
                                      m.Body.Instructions[i].Operand.ToString().Contains("Class::Init") && m.Body.Instructions[i - 1].OpCode == OpCodes.Ldstr)
                        {
                            var stringmd = m.Body.Instructions[i - 1].Operand.ToString();
                            return stringmd;
                        }
                    }
                }
            }
            throw new Exception("Can't find Initer!");
        }

    }
}
