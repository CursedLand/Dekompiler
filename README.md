# Dekompiler
Dekompiler is work-in-progress .NET decompiler that converts Cil to C# code.


# Showcase

![corlib_ginterface](https://github.com/CursedLand/Dekompiler/blob/main/showcase/corlib_ginterface.png)
![dekompiler_core_ldnull](https://github.com/CursedLand/Dekompiler/blob/main/showcase/dekompiler_core_ldnull.png)
![dekompiler_core_newarr](https://github.com/CursedLand/Dekompiler/blob/main/showcase/dekompiler_core_newarr.png)
![koi_config](https://github.com/CursedLand/Dekompiler/blob/main/showcase/koi_config.png)
![koi_rt_cmpdwrd](https://github.com/CursedLand/Dekompiler/blob/main/showcase/koi_rt_cmpdwrd.png)
![koi_rt_vmctx](https://github.com/CursedLand/Dekompiler/blob/main/showcase/koi_rt_vmctx.png)
![koi_vminstance](https://github.com/CursedLand/Dekompiler/blob/main/showcase/koi_vminstance.png)
![pointers](https://github.com/CursedLand/Dekompiler/blob/main/showcase/pointers.png)
![unsupported](https://github.com/CursedLand/Dekompiler/blob/main/showcase/unsupported.png)
![x86_decoded_expressions](https://github.com/CursedLand/Dekompiler/blob/main/showcase/x86_decoded_expressions.png)

# Issues
- Dekompiler do not use Ast to process the assemblies.
- Dekompiler do not optimize CompilerGenerated members.
- Dekompiler do not support loading assemblies from disk (this means you need to load the dependencies as-well).
- Dekompiler do not support control-flow codes.
- Dekompiler do not support exception-handlers codes.
- Dekompiler do not support nullability.
- Dekompiler do not support operator methods (i.e op_Explicit, op_Add, op_Implicit).
- Dekompiler do not support using-section in decompilation result.
- Dekompiler do not support rendering of couple TypeSignatures.
- Dekompiler do not support basic code inlining nor optimization.
- Dekompiler do not support dead code removal.
- Dekompiler do not support any Attributes applied to member-definition(s).
- Dekompiler might introduce mistakes in member-definition(s) accessibility flags.
- Dekompiler might fails to render Generics in some cases due to incorrect implementation.

# Credits
[Washi](https://github.com/Washi1337) for [AsmResolver](https://github.com/Washi1337/AsmResolver)
