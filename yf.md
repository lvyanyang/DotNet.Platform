
# 前言

## 简介

<div style="color:red">
JavaScript是一种轻量级、嵌入式的脚本语言。所谓“脚本语言”，只能用来编写控制其他大型应用程序的“脚本”。
</div>

它本身提供的核心语法，规模相当小，只能用来做一些数学和逻辑运算。JavaScript本身不提供任何与I/O（输入/输出）相关的API，都要靠宿主环境（host）提供，所以JavaScript只合适嵌入更大型的应用程序环境，去调用宿主环境提供的底层API。

目前，已经嵌入JavaScript的宿主环境有多种，最常见的环境就是浏览器，另外还有服务器环境，也就是Node项目。

从语法角度看，JavaScript语言是一种“对象模型”语言。各种宿主环境通过这个模型，描述自己的功能和操作接口，从而通过JavaScript控制这些功能。

JavaScript的核心语法部分相当精简，只包括两个部分：基本的语法构造（比如操作符、控制结构、语句）和标准库（就是一系列具有各种功能的对象比如`Array`、`Date`、`Math`等）。除此之外，各种宿主环境提供额外的API（即只能在该环境使用的接口），以便JavaScript调用。

本文的内容基于ECMAScript 5.1版本，这是使用最广泛的版本，也是学习JavaScript的基础。

## 历史

1997年7月，ECMAScript 1.0发布。

1998年6月，ECMAScript 2.0版发布。

1999年12月，ECMAScript 3.0版发布，成为JavaScript的通行标准，得到了广泛支持。

2007年10月，ECMAScript 4.0版草案发布，对3.0版做了大幅升级，预计次年8月发布正式版本。草案发布后，由于4.0版的目标过于激进，各方对于是否通过这个标准，发生了严重分歧。以Yahoo、Microsoft、Google为首的大公司，反对JavaScript的大幅升级，主张小幅改动；以JavaScript创造者Brendan Eich为首的Mozilla公司，则坚持当前的草案。

2008年7月，由于对于下一个版本应该包括哪些功能，各方分歧太大，争论过于激进，ECMA开会决定，中止ECMAScript 4.0的开发（即废除了这个版本），将其中涉及现有功能改善的一小部分，发布为ECMAScript 3.1，而将其他激进的设想扩大范围，放入以后的版本，由于会议的气氛，该版本的项目代号起名为Harmony（和谐）。会后不久，ECMAScript 3.1就改名为ECMAScript 5。

2009年12月，ECMAScript 5.0版正式发布。Harmony项目则一分为二，一些较为可行的设想定名为JavaScript.next继续开发，后来演变成ECMAScript 6；一些不是很成熟的设想，则被视为JavaScript.next.next，在更远的将来再考虑推出。TC39的总体考虑是，ECMAScript 5与ECMAScript 3基本保持兼容，较大的语法修正和新功能加入，将由JavaScript.next完成。当时，JavaScript.next指的是ECMAScript 6。第六版发布以后，将指ECMAScript 7。TC39预计，ECMAScript 5会在2013年的年中成为JavaScript开发的主流标准，并在此后五年中一直保持这个位置。

2011年6月，ECMAscript 5.1版发布，并且成为ISO国际标准（ISO/IEC 16262:2011）。到了2012年底，所有主要浏览器都支持ECMAScript 5.1版的全部功能。

2013年3月，ECMAScript 6草案冻结，不再添加新功能。新的功能设想将被放到ECMAScript 7。

2013年12月，ECMAScript 6草案发布。然后是12个月的讨论期，听取各方反馈。

2015年6月，ECMAScript 6正式发布，并且更名为“ECMAScript 2015”。这是因为TC39委员会计划，以后每年发布一个ECMAScirpt的版本，下一个版本在2016年发布，称为“ECMAScript 2016”。

除了ECMAScript的版本，很长一段时间中，Netscape公司（以及继承它的Mozilla基金会）在内部依然使用自己的版本号。这导致了JavaScript有自己不同于ECMAScript的版本号。1996年3月，Navigator 2.0内置了JavaScript 1.0。JavaScript 1.1版对应ECMAScript 1.0，但是直到JavaScript 1.4版才完全兼容ECMAScript 1.0。JavaScript 1.5版完全兼容ECMAScript 3.0。目前的JavaScript 1.8版完全兼容ECMAScript 5。

## 大事记

JavaScript伴随着互联网的发展一起发展。互联网周边技术的快速发展，刺激和推动了JavaScript语言的发展。

1996年，样式表标准CSS第一版发布。

1997年，DHTML（Dynamic HTML，动态HTML）发布，允许动态改变网页内容。这标志着DOM模式（Document Object Model，文档对象模型）正式应用。

1998年，Netscape公司开源了浏览器套件，这导致了Mozilla项目的诞生。几个月后，美国在线（AOL）宣布并购Netscape。

1999年，IE 5部署了XMLHttpRequest接口，允许JavaScript发出HTTP请求，为后来大行其道的Ajax应用创造了条件。

2000年，KDE项目重写了浏览器引擎KHTML，为后来的WebKit和Blink引擎打下基础。这一年的10月23日，KDE 2.0发布，第一次将KHTML浏览器包括其中。

2001年，微软公司时隔5年之后，发布了IE浏览器的下一个版本Internet Explorer 6。这是当时最先进的浏览器，它后来统治了浏览器市场多年。

2001年，Douglas Crockford提出了JSON格式，用于取代XML格式，进行服务器和网页之间的数据交换。JavaScript可以原生支持这种格式，不需要额外部署代码。

2002年，Mozilla项目发布了它的浏览器的第一版，后来起名为Firefox。

2003年，苹果公司发布了Safari浏览器的第一版。

2004年，Google公司发布了Gmail，促成了互联网应用程序（Web Application）这个概念的诞生。由于Gmail是在4月1日发布的，很多人起初以为这只是一个玩笑。

2004年，Dojo框架诞生，为不同浏览器提供了同一接口，并为主要功能提供了便利的调用方法。这标志着JavaScript编程框架的时代开始来临。

2005年，苹果公司在KHTML引擎基础上，建立了WebKit引擎。

2005年，Ajax方法（Asynchronous JavaScript and XML）正式诞生，Jesse James Garrett发明了这个词汇。它开始流行的标志是，2月份发布的Google Maps项目大量采用该方法。它几乎成了新一代网站的标准做法，促成了Web 2.0时代的来临。

2005年，Apache基金会发布了CouchDB数据库。这是一个基于JSON格式的数据库，可以用JavaScript函数定义视图和索引。它在本质上有别于传统的关系型数据库，标识着NoSQL类型的数据库诞生。

**2006年，jQuery函数库诞生，作者为John Resig。jQuery为操作网页DOM结构提供了非常强大易用的接口，成为了使用最广泛的函数库，并且让JavaScript语言的应用难度大大降低，推动了这种语言的流行。**

2006年，微软公司发布IE 7，标志重新开始启动浏览器的开发。

2006年，Google推出 Google Web Toolkit 项目（缩写为GWT），提供Java编译成JavaScript的功能，开创了将其他语言转为JavaScript的先河。

2007年，Webkit引擎在iPhone手机中得到部署。它最初基于KDE项目，2003年苹果公司首先采用，2005年开源。这标志着JavaScript语言开始能在手机中使用了，意味着有可能写出在桌面电脑和手机中都能使用的程序。

2008年，V8编译器诞生。这是Google公司为Chrome浏览器而开发的，它的特点是让JavaScript的运行变得非常快。它提高了JavaScript的性能，推动了语法的改进和标准化，改变外界对JavaScript的不佳印象。同时，V8是开源的，任何人想要一种快速的嵌入式脚本语言，都可以采用V8，这拓展了JavaScript的应用领域。

2009年，Node.js项目诞生，创始人为Ryan Dahl，它标志着JavaScript可以用于服务器端编程，从此网站的前端和后端可以使用同一种语言开发。并且，Node.js可以承受很大的并发流量，使得开发某些互联网大规模的实时应用变得容易。

2009年，PhoneGap项目诞生，它将HTML5和JavaScript引入移动设备的应用程序开发，主要针对iOS和Android平台，使得JavaScript可以用于跨平台的应用程序开发。

2010年，三个重要的项目诞生，分别是NPM、BackboneJS和RequireJS，标志着JavaScript进入模块化开发的时代。

2011年，微软公司发布Windows 8操作系统，将JavaScript作为应用程序的开发语言之一，直接提供系统支持。

2012年，微软发布TypeScript语言。该语言被设计成JavaScript的超集，这意味着所有JavaScipt程序，都可以不经修改地在TypeScript中运行。同时，TypeScript添加了很多新的语法特性，主要目的是为了开发大型程序，然后还可以被编译成JavaScript运行。

2013年，Mozilla基金会发布手机操作系统Firefox OS，该操作系统的整个用户界面都使用JavaScript。

2013年，ECMA正式推出JSON的[国际标准](http://www.ecma-international.org/publications/standards/Ecma-404.htm)，这意味着JSON格式已经变得与XML格式一样重要和正式了。

2014年11月，由于对Joyent公司垄断Node项目、以及该项目进展缓慢的不满，一部分核心开发者离开了Node.js，创造了io.js项目，这是一个更开放、更新更频繁的Node.js版本，很短时间内就发布到了2.0版。三个月后，Joyent公司宣布放弃对Node项目的控制，将其转交给新成立的开放性质的Node基金会。随后，io.js项目宣布回归Node，两个版本将合并。

2015年5月，Node模块管理器npm超越CPAN，标志着JavaScript成为世界上软件模块最多的语言。

2015年6月，ECMA标准化组织正式批准了ECMAScript 6语言标准，定名为《ECMAScript 2015 标准》。JavaScript语言正式进入了下一个阶段，成为一种企业级的、开发大规模应用的语言。这个标准从提出到批准，历时10年，而JavaScript语言从诞生至今也已经20年了。

2015年6月，Mozilla在asm.js的基础上发布WebAssembly项目。这是一种JavaScript语言编译后的二进制格式，类似于Java的字节码，有利于移动设备加载JavaScript脚本，解析速度提高了20+倍。这意味着将来的软件，会发布JavaScript二进制包。

2016年6月，《ECMAScript 2016 标准》发布。

# 语法

## 变量

### 定义
```javascript
var a = 1;
var b;
b // undefined
```
变量的声明和赋值，是分开的两个步骤，如果只是声明变量而没有赋值，则该变量的值是`undefined`。`undefined`是一个JavaScript关键字，表示“无定义”。

如果变量赋值的时候，忘了写`var`命令，这条语句也是有效的。但是，不写`var`的做法，不利于表达意图，而且容易不知不觉地创建全局变量，所以建议总是使用`var`命令声明变量。

如果一个变量没有声明就直接使用，JavaScript会报错，告诉你变量未定义。
```javascript
x
// ReferenceError: x is not defined
```
可以在同一条`var`命令中声明多个变量。
```javascript
var a, b;
```

如果多次声明同样的变量，则会覆盖掉前面的值。
```javascript
var x = 1;
var x = 2;

// 等同于

var x = 1;
var x;
x = 2;
```

### 变量提升
  JavaScript引擎的工作方式是，先解析代码，获取所有被声明的变量，然后再一行一行地运行。这造成的结果，就是所有的变量的声明语句，都会被提升到代码的头部，这就叫做变量提升。
```javascript
console.log(a);
var a = 1;

// 等同于

var a;
console.log(a);
a = 1;
//结果是显示`undefined`，表示变量a已声明，但还未赋值。
```
请注意，变量提升只对`var`命令声明的变量有效，如果一个变量不是用`var`命令声明的，就不会发生变量提升。
```
console.log(b);
b = 1;
//提示“ReferenceError: b is not defined”，即变量b未声明
//这是因为b不是用var命令声明的，JavaScript引擎不会将其提升，而只是视为对顶层对象的b属性的赋值。
```

## 标识符
标识符（identifier）是用来识别具体对象的一个名称。最常见的标识符就是变量名，以及后面要提到的函数名。JavaScript语言的标识符对大小写敏感，所以`a`和`A`是两个不同的标识符。

标识符有一套命名规则，不符合规则的就是非法标识符。JavaScript引擎遇到非法标识符，就会报错。

简单说，标识符命名规则如下：

- 第一个字符，可以是任意Unicode字母（包括英文字母和其他语言的字母），以及美元符号（`$`）和下划线（`_`）。
- 第二个字符及后面的字符，除了Unicode字母、美元符号和下划线，还可以用数字`0-9`。

下面这些都是合法的标识符。

```javascript
arg0
_tmp
$elem
π
```

下面这些则是不合法的标识符。

```javascript
1a  // 第一个字符不能是数字
23  // 同上
***  // 标识符不能包含星号
a+b  // 标识符不能包含加号
-d  // 标识符不能包含减号或连词线
```

中文是合法的标识符，可以用作变量名。

```javascript
var 临时变量 = 1;
```

> JavaScript有一些保留字，不能用作标识符：arguments、break、case、catch、class、const、continue、debugger、default、delete、do、else、enum、eval、export、extends、false、finally、for、function、if、implements、import、in、instanceof、interface、let、new、null、package、private、protected、public、return、static、super、switch、this、throw、true、try、typeof、var、void、while、with、yield。

另外，还有三个词虽然不是保留字，但是因为具有特别含义，也不应该用作标识符：`Infinity`、`NaN`、`undefined`。

## 语句

JavaScript程序的执行单位为行（line），也就是一行一行地执行。一般情况下，每一行就是一个语句。

语句（statement）是为了完成某种任务而进行的操作，比如下面就是一行赋值语句：

```javascript
var a = 1 + 3;
```

凡是JavaScript语言中预期为值的地方，都可以使用表达式。比如，赋值语句的等号右边，预期是一个值，因此可以放置各种表达式。一条语句可以包含多个表达式。

语句以分号结尾，一个分号就表示一个语句结束。多个语句可以写在一行内。

```javascript
var a = 1 + 3 ; var b = 'abc';
```

分号前面可以没有任何内容，JavaScript引擎将其视为空语句。

```javascript
;;;
```

表达式不需要分号结尾。一旦在表达式后面添加分号，则JavaScript引擎就将表达式视为语句，这样会产生一些没有任何意义的语句。

```javascript
1 + 3;
'abc';
```

上面两行语句有返回值，但是没有任何意义，因为只是返回一个单纯的值，没有任何其他操作。

## 注释

源码中被JavaScript引擎忽略的部分就叫做注释，它的作用是对代码进行解释。Javascript提供两种注释：一种是单行注释，用//起头；另一种是多行注释，放在/\* 和 \*/之间。

```javascript
// 这是单行注释

/*
 这是
 多行
 注释
*/
```

## 条件语句
条件语句提供一种语法构造，只有满足某个条件，才会执行相应的语句。JavaScript提供`if`结构和`switch`结构，完成条件判断。

- if 结构

```javascript
if (expression){
  statement;
}
```

- if...else结构

```javascript
if (expression){
  statement;
}
else {
  statement;
}
```

- if...else...if...else结构

```javascript
if (expression){
  statement;
}
else if (expression){
  statement;
}
else {
  statement;
}
```

- switch结构

```javascript
switch (expression) {
  case "android":
    // ...
    break;
  case "apple":
    // ...
    break;
  default:
    // ...
}
```

- 三元运算符 ?:

```javascript
(condition) ? expr1 : expr2
```

## 循环语句
循环语句用于重复执行某个操作，它有多种形式。

- while循环
```javascript
while (expression){
  statement;
}
```

- for循环
```javascript
for (initialize; test; increment) {
  statement
}
```

- do...while循环
```javascript
do {
  statement
} while (expression);
```

- break语句和continue语句
  `break`语句和`continue`语句都具有跳转作用，可以让代码不按既有的顺序执行。

  `break`语句用于跳出代码块或循环。
  `continue`语句用于立即终止本轮循环，返回循环结构的头部，开始下一轮循环。
  如果存在多重循环，不带参数的`break`语句和`continue`语句都只针对最内层循环。

## 数据类型
JavaScript 语言的每一个值，都属于某一种数据类型。JavaScript 的数据类型，共有六种。


- 数值（number）：整数和小数（比如1和3.14）
- 字符串（string）：字符组成的文本（比如"Hello World"）
- 布尔值（boolean）：`true`（真）和  `false`（假）两个特定值
- `undefined`：表示“未定义”或不存在，即由于目前没有定义，所以此处暂时没有任何值
- `null`：表示无值，即此处的值就是“无”的状态。
- 对象（object）：各种值组成的集合

通常，我们将数值、字符串、布尔值称为原始类型（primitive type）的值，即它们是最基本的数据类型，不能再细分了。而将对象称为合成类型（complex type）的值，因为一个对象往往是多个原始类型的值的合成，可以看作是一个存放各种值的容器。至于`undefined`和`null`，一般将它们看成两个特殊值。

对象又可以分成三个子类型。

- 狭义的对象（object）
- 数组（array）
- 函数（function）

狭义的对象和数组是两种不同的数据组合方式，而函数其实是处理数据的方法。JavaScript把函数当成一种数据类型，可以像其他类型的数据一样，进行赋值和传递，这为编程带来了很大的灵活性，体现了JavaScript作为“函数式语言”的本质。

这里需要明确的是，JavaScript的所有数据，都可以视为广义的对象。不仅数组和函数属于对象，就连原始类型的数据（数值、字符串、布尔值）也可以用对象方式调用。

## 获取对象类型

JavaScript有三种方法，可以确定一个值到底是什么类型。

- `typeof`运算符
- `instanceof`运算符
- `Object.prototype.toString`方法

---
一、`typeof`运算符可以返回一个值的数据类型，可能有以下结果。

**（1）原始类型**

数值、字符串、布尔值分别返回`number`、`string`、`boolean`。

```javascript
typeof 123 // "number"
typeof '123' // "string"
typeof false // "boolean"
```

**（2）函数**

函数返回`function`。

```javascript
function f() {}
typeof f
// "function"
```

**（3）undefined**

`undefined`返回`undefined`。

```javascript
typeof undefined
// "undefined"
```

利用这一点，typeof可以用来检查一个没有声明的变量，而不报错。

```javascript
v
// ReferenceError: v is not defined

typeof v
// "undefined"
```

上面代码中，变量`v`没有用`var`命令声明，直接使用就会报错。但是，放在`typeof`后面，就不报错了，而是返回`undefined`。

实际编程中，这个特点通常用在判断语句。

```javascript
// 错误的写法
if (v) {
  // ...
}
// ReferenceError: v is not defined

// 正确的写法
if (typeof v === "undefined") {
  // ...
}
```

**（4）其他**

除此以外，其他情况都返回`object`。

```javascript
typeof window // "object"
typeof {} // "object"
typeof [] // "object"
typeof null // "object"
```

从上面代码可以看到，空数组（`[]`）的类型也是`object`，这表示在JavaScript内部，数组本质上只是一种特殊的对象。

另外，`null`的类型也是`object`，这是由于历史原因造成的。1995年JavaScript语言的第一版，所有值都设计成32位，其中最低的3位用来表述数据类型，`object`对应的值是`000`。当时，只设计了五种数据类型（对象、整数、浮点数、字符串和布尔值），完全没考虑`null`，只把它当作`object`的一种特殊值，32位全部为0。这是`typeof null`返回`object`的根本原因。

为了兼容以前的代码，后来就没法修改了。这并不是说`null`就属于对象，本质上`null`是一个类似于`undefined`的特殊值。

既然`typeof`对数组（array）和对象（object）的显示结果都是`object`，那么怎么区分它们呢？`instanceof`运算符可以做到。

---
二、`instanceof`运算符返回一个布尔值，表示指定对象是否为某个构造函数的实例。

```javascript
var o = {};
var a = [];

o instanceof Array // false
a instanceof Array // true
a instanceof Object // true
```
注意，`instanceof`运算符只能用于对象，不适用原始类型的值。

```javascript
var s = 'hello';
s instanceof String // false
```

上面代码中，字符串不是`String`对象的实例（因为字符串不是对象），所以返回`false`。

此外，`undefined`和`null`不是对象，所以`instanceOf`运算符总是返回`false`。

```javascript
undefined instanceof Object // false
null instanceof Object // false
```

```javascript
var v = new Vehicle();
v instanceof Vehicle // true
```

上面代码中，对象`v`是构造函数`Vehicle`的实例，所以返回`true`。

`instanceof`运算符的左边是实例对象，右边是构造函数。它的运算实质是检查右边构建函数的原型对象，是否在左边对象的原型链上。因此，下面两种写法是等价的。

```javascript
v instanceof Vehicle
// 等同于
Vehicle.prototype.isPrototypeOf(v)
```

由于`instanceof`对整个原型链上的对象都有效，因此同一个实例对象，可能会对多个构造函数都返回`true`。

```javascript
var d = new Date();
d instanceof Date // true
d instanceof Object // true
```
---
三、`Object.prototype.toString`方法返回对象的类型字符串，因此可以用来判断一个值的类型。

不同数据类型的`Object.prototype.toString`方法返回值如下。

- 数值：返回`[object Number]`。
- 字符串：返回`[object String]`。
- 布尔值：返回`[object Boolean]`。
- undefined：返回`[object Undefined]`。
- null：返回`[object Null]`。
- 数组：返回`[object Array]`。
- arguments对象：返回`[object Arguments]`。
- 函数：返回`[object Function]`。
- Error对象：返回`[object Error]`。
- Date对象：返回`[object Date]`。
- RegExp对象：返回`[object RegExp]`。
- 其他对象：返回`[object Object]`。

也就是说，`Object.prototype.toString`可以得到一个实例对象的构造函数。

```javascript
Object.prototype.toString.call(2) // "[object Number]"
Object.prototype.toString.call('') // "[object String]"
Object.prototype.toString.call(true) // "[object Boolean]"
Object.prototype.toString.call(undefined) // "[object Undefined]"
Object.prototype.toString.call(null) // "[object Null]"
Object.prototype.toString.call(Math) // "[object Math]"
Object.prototype.toString.call({}) // "[object Object]"
Object.prototype.toString.call([]) // "[object Array]"
```

利用这个特性，可以写出一个比`typeof`运算符更准确的类型判断函数。

```javascript
var type = function (o){
  var s = Object.prototype.toString.call(o);
  return s.match(/\[object (.*?)\]/)[1].toLowerCase();
};

type({}); // "object"
type([]); // "array"
type(5); // "number"
type(null); // "null"
type(); // "undefined"
type(/abcd/); // "regex"
type(new Date()); // "date"
```

## 布尔值

布尔值代表“真”和“假”两个状态。“真”用关键字`true`表示，“假”用关键字`false`表示。布尔值只有这两个值。

下列运算符会返回布尔值：

- 两元逻辑运算符： `&&` (And)，`||` (Or)
- 前置逻辑运算符： `!` (Not)
- 相等运算符：`===`，`!==`，`==`，`!=`
- 比较运算符：`>`，`>=`，`<`，`<=`

如果JavaScript预期某个位置应该是布尔值，会将该位置上现有的值自动转为布尔值。转换规则是除了下面六个值被转为`false`，其他值都视为`true`。

- `undefined`
- `null`
- `false`
- `0`
- `NaN`
- `""`或`''`（空字符串）

布尔值往往用于程序流程的控制，请看一个例子。

```javascript
if ('') {
  console.log(true);
}
// 没有任何输出
```

上面代码的`if`命令后面的判断条件，预期应该是一个布尔值，所以JavaScript自动将空字符串，转为布尔值`false`，导致程序不会进入代码块，所以没有任何输出。

需要特别注意的是，空数组（`[]`）和空对象（`{}`）对应的布尔值，都是`true`。

```javascript
if ([]) {
  console.log(true);
}
// true

if ({}) {
  console.log(true);
}
// true
```

注意下面的代码，下面的写法并不会发生错误

```javascript
var a = 1;
if(a=2){
  console.log('hello');
}
//hello
```

使用双重的否运算符（`!`）也可以将任意值转为对应的布尔值。

```javascript
!!undefined // false
!!null // false
!!0 // false
!!'' // false
!!NaN // false
!!1 // true
!!'false' // true
!![] // true
!!{} // true
!!function(){} // true
!!/foo/ // true
```
`Boolean`对象除了可以作为构造函数，还可以单独使用，将任意值转为布尔值。这时`Boolean`就是一个单纯的工具方法。

```javascript
Boolean(undefined) // false
Boolean(null) // false
Boolean(0) // false
Boolean('') // false
Boolean(NaN) // false

Boolean(1) // true
Boolean('false') // true
Boolean([]) // true
Boolean({}) // true
Boolean(function () {}) // true
Boolean(/foo/) // true
```

上面代码中几种得到`true`的情况，都值得认真记住。

最后，对于一些特殊值，`Boolean`对象前面加不加`new`，会得到完全相反的结果，必须小心。

```javascript
if (Boolean(false)) {
  console.log('true');
} // 无输出

if (new Boolean(false)) {
  console.log('true');
} // true

if (Boolean(null)) {
  console.log('true');
} // 无输出

if (new Boolean(null)) {
  console.log('true');
} // true
```
## null 和 undefined

`null`与`undefined`都可以表示“没有”，含义非常相似。将一个变量赋值为`undefined`或`null`，老实说，语法效果几乎没区别。

```javascript
var a = undefined;
// 或者
var a = null;
```

上面代码中，`a`变量分别被赋值为`undefined`和`null`，这两种写法的效果几乎等价。

在`if`语句中，它们都会被自动转为`false`，相等运算符（`==`）甚至直接报告两者相等。

```javascript
if (!undefined) {
  console.log('undefined is false');
}
// undefined is false

if (!null) {
  console.log('null is false');
}
// null is false

undefined == null
// true
```

从上面代码可见，两者的行为是何等相似！谷歌公司开发的 JavaScript 语言的替代品 Dart 语言，就明确规定只有`null`，没有`undefined`！

既然含义与用法都差不多，为什么要同时设置两个这样的值，这不是无端增加复杂度，令初学者困扰吗？这与历史原因有关。

1995年 JavaScript 诞生时，最初像Java一样，只设置了`null`作为表示"无"的值。根据C语言的传统，`null`被设计成可以自动转为`0`。

```javascript
Number(null) // 0
5 + null // 5
```

但是，JavaScript的设计者Brendan Eich，觉得这样做还不够，有两个原因。首先，`null`像在Java里一样，被当成一个对象。但是，JavaScript的值分成原始类型和合成类型两大类，Brendan Eich觉得表示"无"的值最好不是对象。其次，JavaScript的最初版本没有包括错误处理机制，发生数据类型不匹配时，往往是自动转换类型或者默默地失败。Brendan Eich觉得，如果`null`自动转为0，很不容易发现错误。

因此，Brendan Eich又设计了一个`undefined`。他是这样区分的：`null`是一个表示"无"的对象，转为数值时为`0`；`undefined`是一个表示"无"的原始值，转为数值时为`NaN`。

```javascript
Number(undefined) // NaN
5 + undefined // NaN
```

但是，这样的区分在实践中很快就被证明不可行。目前`null`和`undefined`基本是同义的，只有一些细微的差别。

`null`的特殊之处在于，JavaScript把它包含在对象类型（object）之中。

```javascript
typeof null // "object"
```

上面代码表示，查询`null`的类型，JavaScript返回`object`（对象）。

这并不是说null的数据类型就是对象，而是JavaScript早期部署中的一个约定俗成，其实不完全正确，后来再想改已经太晚了，会破坏现存代码，所以一直保留至今。

注意，JavaScript的标识名区分大小写，所以`undefined`和`null`不同于`Undefined`和`Null`（或者其他仅仅大小写不同的词形），后者只是普通的变量名。

对于`null`和`undefined`，可以大致可以像下面这样理解。

`null`表示空值，即该处的值现在为空。调用函数时，某个参数未设置任何值，这时就可以传入`null`。比如，某个函数接受引擎抛出的错误作为参数，如果运行过程中未出错，那么这个参数就会传入`null`，表示未发生错误。

`undefined`表示“未定义”，下面是返回`undefined`的典型场景。

```javascript
// 变量声明了，但没有赋值
var i;
i // undefined

// 调用函数时，应该提供的参数没有提供，该参数等于undefined
function f(x) {
  return x;
}
f() // undefined

// 对象没有赋值的属性
var o = new Object();
o.p // undefined

// 函数没有返回值时，默认返回undefined
function f() {}
f() // undefined
```

## 整数和浮点数

`Number`对象是数值对应的包装对象，可以作为构造函数使用，也可以作为工具函数使用。

作为构造函数时，它用于生成值为数值的对象。

```javascript
var n = new Number(1);
typeof n // "object"
```

上面代码中，`Number`对象作为构造函数使用，返回一个值为`1`的对象。

作为工具函数时，它可以将任何类型的值转为数值。

```javascript
Number(true) // 1
```

上面代码将布尔值`true`转为数值`1`。

由于浮点数不是精确的值，所以涉及小数的比较和运算要特别小心。

```javascript
0.1 + 0.2
0.30000000000000004
0.1 + 0.2 === 0.3
// false

0.3 / 0.1
// 2.9999999999999996

(0.3 - 0.2) === (0.2 - 0.1)
// false
```

### Number对象属性

`Number`对象拥有以下一些属性。

- `Number.POSITIVE_INFINITY`：正的无限，指向`Infinity`。
- `Number.NEGATIVE_INFINITY`：负的无限，指向`-Infinity`。
- `Number.NaN`：表示非数值，指向`NaN`。
- `Number.MAX_VALUE`：表示最大的正数，相应的，最小的负数为`-Number.MAX_VALUE`。
- `Number.MIN_VALUE`：表示最小的正数（即最接近0的正数，在64位浮点数体系中为`5e-324`），相应的，最接近0的负数为`-Number.MIN_VALUE`。
- `Number.MAX_SAFE_INTEGER`：表示能够精确表示的最大整数，即`9007199254740991`。
- `Number.MIN_SAFE_INTEGER`：表示能够精确表示的最小整数，即`-9007199254740991`。

```javascript
Number.POSITIVE_INFINITY // Infinity
Number.NEGATIVE_INFINITY // -Infinity
Number.NaN // NaN

Number.MAX_VALUE
// 1.7976931348623157e+308
Number.MAX_VALUE < Infinity
// true

Number.MIN_VALUE
// 5e-324
Number.MIN_VALUE > 0
// true

Number.MAX_SAFE_INTEGER // 9007199254740991
Number.MIN_SAFE_INTEGER // -9007199254740991
```
### 数值范围

JavaScript能够表示的数值范围为2<sup>1024</sup>到2<sup>-1023</sup>（开区间），超出这个范围的数无法表示。

JavaScript提供Number对象的`MAX_VALUE`和`MIN_VALUE`属性表示（参见《Number对象》一节）。

```javascript
Number.MAX_VALUE // 1.7976931348623157e+308
Number.MIN_VALUE // 5e-324
```

### 特殊数值

**（1）NaN**

`NaN`是JavaScript的特殊值，表示“非数字”（Not a Number），主要出现在将字符串解析成数字出错的场合。

非数字转换为数字时会返回`NaN`
```javascript
5 - 'x' // NaN
```

`NaN`不是一种独立的数据类型，而是一种特殊数值，它的数据类型依然属于`Number`，使用`typeof`运算符可以看得很清楚。

```javascript
typeof NaN // 'number'
```

`NaN`不等于任何值，包括它本身。
```javascript
NaN === NaN // false
```
`NaN`在布尔运算时被当作`false`。
`NaN`与任何数（包括它自己）的运算，得到的都是`NaN`。
`isNaN`方法可以用来判断一个值是否为`NaN`。
但是，`isNaN`只对数值有效，如果传入其他值，会被先转成数值。比如，传入字符串的时候，字符串会被先转成`NaN`，所以最后返回`true`，这一点要特别引起注意。也就是说，`isNaN`为`true`的值，有可能不是`NaN`，而是一个字符串。
因此，使用`isNaN`之前，最好判断一下数据类型。

**（2）Infinity**

`Infinity`表示“无穷”，用来表示两种场景。一种是一个正的数值太大，或一个负的数值太小，无法表示；另一种是非0数值除以0，得到`Infinity`。
```javascript
// 场景一
Math.pow(2, Math.pow(2, 100))
// Infinity

// 场景二
0 / 0 // NaN
1 / 0 // Infinity
```

上面代码中，第一个场景是一个表达式的计算结果太大，超出了JavaScript能够表示的范围，因此返回`Infinity`。第二个场景是`0`除以`0`会得到`NaN`，而非0数值除以`0`，会返回`Infinity`。

`Infinity`有正负之分，`Infinity`表示正的无穷，`-Infinity`表示负的无穷。

`isFinite`函数返回一个布尔值，检查某个值是不是正常数值，而不是`Infinity`。

```javascript
isFinite(Infinity) // false
isFinite(-1) // true
isFinite(true) // true
isFinite(NaN) // false
```

上面代码表示，如果对`NaN`使用`isFinite`函数，也返回`false`，表示`NaN`不是一个正常值。

### 全局方法

**（1）parseInt**

用于将字符串转为整数。
如果字符串头部有空格，空格会被自动去除。

```javascript
parseInt('   81') // 81
```
如果`parseInt`的参数不是字符串，则会先转为字符串再转换。
字符串转为整数的时候，是一个个字符依次转换，如果遇到不能转为数字的字符，就不再进行下去，返回已经转好的部分。

```javascript
parseInt('8a') // 8
parseInt('12**') // 12
parseInt('12.34') // 12
parseInt('15e2') // 15
parseInt('15px') // 15
```

如果字符串的第一个字符不能转化为数字（后面跟着数字的正负号除外），返回`NaN`。

```javascript
parseInt('abc') // NaN
parseInt('.3') // NaN
parseInt('') // NaN
parseInt('+') // NaN
parseInt('+1') // 1
```

`parseInt`的返回值只有两种可能，不是一个十进制整数，就是`NaN`。

**（2）parseFloat**

用于将一个字符串转为浮点数。


## 字符串

字符串就是零个或多个排在一起的字符，放在单引号或双引号之中。

单引号字符串的内部，可以使用双引号。双引号字符串的内部，可以使用单引号。

```javascript
'key = "value"'
"It's a long journey"
```
很多项目约定JavaScript语言的字符串只使用单引号，本教程就遵守这个约定。当然，只使用双引号也完全可以。重要的是，坚持使用一种风格，不要两种风格混合。

反斜杠（\）在字符串内有特殊含义，用来表示一些特殊字符，所以又称为转义符。

需要用反斜杠转义的特殊字符，主要有下面这些：

- `\0` null（\u0000）
- `\b` 后退键（\u0008）
- `\f` 换页符（\u000C）
- `\n` 换行符（\u000A）
- `\r` 回车键（\u000D）
- `\t` 制表符（\u0009）
- `\v` 垂直制表符（\u000B）
- `\'` 单引号（\u0027）
- `\"` 双引号（\u0022）
- \\ 反斜杠（\u005C）

上面这些字符前面加上反斜杠，都表示特殊含义。

`length`属性返回字符串的长度，只读属性。

`String`对象是JavaScript原生提供的三个包装对象之一，用来生成字符串的包装对象。

```javascript
var s1 = 'abc';
var s2 = new String('abc');

typeof s1 // "string"
typeof s2 // "object"

s2.valueOf() // "abc"
```

上面代码中，变量`s1`是字符串，`s2`是对象。由于`s2`是对象，所以有自己的方法，`valueOf`方法返回的就是它所包装的那个字符串。

实际上，字符串的包装对象是一个类似数组的对象（即很像数组，但是实质上不是数组）。

```javascript
new String("abc")
// String {0: "a", 1: "b", 2: "c", length: 3}
```

除了用作构造函数，`String`对象还可以当作工具方法使用，将任意类型的值转为字符串。

```javascript
String(true) // "true"
String(5) // "5"
```

上面代码将布尔值`ture`和数值`5`，分别转换为字符串。

### 属性方法

- length属性  返回字符串的长度。

- substr()

  `substr`方法用于从原字符串取出子字符串并返回，不改变原字符串。

  `substr`方法的第一个参数是子字符串的开始位置(从0开始)，第二个参数是子字符串的长度。

  ```
  'JavaScript'.substr(4, 6) // "Script"
  ```

  如果省略第二个参数，则表示子字符串一直到原字符串的结束。

  ```javascript
  'JavaScript'.substr(4) // "Script"
  ```

  如果第一个参数是负数，表示倒数计算的字符位置。如果第二个参数是负数，将被自动转为0，因此会返回空字符串。

  ```javascript
  'JavaScript'.substr(-6) // "Script"
  'JavaScript'.substr(4, -1) // ""
  ```

  上面代码的第二个例子，由于参数`-1`自动转为`0`，表示子字符串长度为0，所以返回空字符串。

- indexOf()，lastIndexOf()

  这两个方法用于确定一个字符串在另一个字符串中的位置，都返回一个整数，表示匹配开始的位置。如果返回`-1`，就表示不匹配。两者的区别在于，`indexOf`从字符串头部开始匹配，`lastIndexOf`从尾部开始匹配。

  ```
  'hello world'.indexOf('o') // 4
  'JavaScript'.indexOf('script') // -1

  'hello world'.lastIndexOf('o') // 7
  ```

  它们还可以接受第二个参数，对于`indexOf`方法，第二个参数表示从该位置开始向后匹配；对于`lastIndexOf`，第二个参数表示从该位置起向前匹配。

  ```
  'hello world'.indexOf('o', 6) // 7
  'hello world'.lastIndexOf('o', 6) // 4
  ```

- trim()

  `trim`方法用于去除字符串两端的空格，返回一个新字符串，不改变原字符串。

  ```
  '  hello world  '.trim()
  // "hello world"
  ```

  该方法去除的不仅是空格，还包括制表符（`\t`、`\v`）、换行符（`\n`）和回车符（`\r`）。

  ```
  '\r\nabc \t'.trim() // 'abc'
  ```

- toLowerCase()，toUpperCase()

  `toLowerCase`方法用于将一个字符串全部转为小写，`toUpperCase`则是全部转为大写。它们都返回一个新字符串，不改变原字符串。

  ```
  'Hello World'.toLowerCase()
  // "hello world"

  'Hello World'.toUpperCase()
  // "HELLO WORLD"
  ```

  这个方法也可以将布尔值或数组转为大写字符串，但是需要通过`call`方法使用。

  ```
  String.prototype.toUpperCase.call(true)
  // 'TRUE'
  String.prototype.toUpperCase.call(['a', 'b', 'c'])
  // 'A,B,C'
  ```

- split()

  `split`方法按照给定规则分割字符串，返回一个由分割出来的子字符串组成的数组。

  ```
  'a|b|c'.split('|') // ["a", "b", "c"]
  ```

  如果分割规则为空字符串，则返回数组的成员是原字符串的每一个字符。

  ```
  'a|b|c'.split('') // ["a", "|", "b", "|", "c"]
  ```

  如果省略参数，则返回数组的唯一成员就是原字符串。

  ```
  'a|b|c'.split() // ["a|b|c"]
  ```

  如果满足分割规则的两个部分紧邻着（即中间没有其他字符），则返回数组之中会有一个空字符串。

  ```
  'a||c'.split('|') // ['a', '', 'c']
  ```

  如果满足分割规则的部分处于字符串的开头或结尾（即它的前面或后面没有其他字符），则返回数组的第一个或最后一个成员是一个空字符串。

  ```
  '|b|c'.split('|') // ["", "b", "c"]
  'a|b|'.split('|') // ["a", "b", ""]
  ```

  `split`方法还可以接受第二个参数，限定返回数组的最大成员数。

  ```
  'a|b|c'.split('|', 0) // []
  'a|b|c'.split('|', 1) // ["a"]
  'a|b|c'.split('|', 2) // ["a", "b"]
  'a|b|c'.split('|', 3) // ["a", "b", "c"]
  'a|b|c'.split('|', 4) // ["a", "b", "c"]
  ```

  上面代码中，`split`方法的第二个参数，决定了返回数组的成员数。

## 对象

对象（object）是JavaScript的核心概念，也是最重要的数据类型。JavaScript的所有数据都可以被视为对象。

简单说，所谓对象，就是一种无序的数据集合，由若干个“键值对”（key-value）构成。

```
var o = {
  p1: 'Hello',
  p2: 'World',
  p3: function(){
    //语句
  }
};
```
### 定义

对象的生成方法，通常有三种方法。除了像上面那样直接使用大括号生成（`{}`），还可以用`new`命令生成一个`Object`对象的实例，或者使用`Object.create`方法生成。

```
var o1 = {};
var o2 = new Object();
var o3 = Object.create(Object.prototype);
```

上面三行语句是等价的。一般来说，第一种采用大括号的写法比较简洁，第二种采用构造函数的写法清晰地表示了意图，第三种写法一般用在需要对象继承的场合。

### 键名

对象的所有键名都是字符串，所以加不加引号都可以。如果键名是数值，会被自动转为字符串。

```
var o = {
  'p': 'Hello World'
};
```
```
var o = {
  '1p': "Hello World",
  'h w': "Hello World",
  'p+q': "Hello World"
};
```

上面对象的三个键名，都不符合标识名的条件，所以必须加上引号。

### 属性

对象的每一个“键名”又称为“属性”（property），它的“键值”可以是任何数据类型。如果一个属性的值为函数，通常把这个属性称为“方法”，它可以像函数那样调用。

```
var o = {
  p: function (x) {
    return 2 * x;
  }
};

o.p(1)
// 2
```

上面的对象就有一个方法`p`，它就是一个函数。

对象的属性之间用逗号分隔，最后一个属性后面可以加逗号（trailing comma），也可以不加。

```
var o = {
  p: 123,
  m: function () { ... },
}
```

上面的代码中`m`属性后面的那个逗号，有或没有都不算错。

属性可以动态创建，不必在对象声明时就指定。

```
var obj = {};
obj.foo = 123;
obj.foo // 123
```

上面代码中，直接对`obj`对象的`foo`属性赋值，结果就在运行时创建了`foo`属性。

### 对象的引用

不同的变量名指向同一个对象，那么它们都是这个对象的引用，也就是说指向同一个内存地址。修改其中一个变量，会影响到其他所有变量。

```
var o1 = {};
var o2 = o1;

o1.a = 1;
o2.a // 1

o2.b = 2;
o1.b // 2
```
但是，这种引用只局限于对象，对于原始类型的数据则是传值引用，也就是说，都是值的拷贝。

```
var x = 1;
var y = x;

x = 2;
y // 1
```

上面的代码中，当`x`的值发生变化后，`y`的值并不变，这就表示`y`和`x`并不是指向同一个内存地址。

### 读取属性

读取对象的属性，有两种方法，一种是使用点运算符，还有一种是使用方括号运算符。

```
var o = {
  p: 'Hello World'
};

o.p // "Hello World"
o['p'] // "Hello World"
```

上面代码分别采用点运算符和方括号运算符，读取属性`p`。

请注意，如果使用方括号运算符，键名必须放在引号里面，否则会被当作变量处理。但是，数字键可以不加引号，因为会被当作字符串处理。

```
var o = {
  0.7: 'Hello World'
};

o['0.7'] // "Hello World"
o[0.7] // "Hello World"
```

方括号运算符内部可以使用表达式。

```
o['hello' + ' world']
o[3 + 3]
```

### 检查变量是否声明

如果读取一个不存在的键，会返回`undefined`，而不是报错。可以利用这一点，来检查一个全局变量是否被声明。

```
// 检查a变量是否被声明
if (a) {...} // 报错

if (window.a) {...} // 不报错
if (window['a']) {...} // 不报错
```

上面的后二种写法之所以不报错，是因为在浏览器环境，所有全局变量都是`window`对象的属性。`window.a`的含义就是读取`window`对象的`a`属性，如果该属性不存在，就返回`undefined`，并不会报错。

### 属性赋值

点运算符和方括号运算符，不仅可以用来读取值，还可以用来赋值。

```
o.p = 'abc';
o['p'] = 'abc';
```

上面代码分别使用点运算符和方括号运算符，对属性p赋值。

JavaScript允许属性的“后绑定”，也就是说，你可以在任意时刻新增属性，没必要在定义对象的时候，就定义好属性。

```
var o = { p: 1 };

// 等价于

var o = {};
o.p = 1;
```
### 查看所有属性

查看一个对象本身的所有属性，可以使用`Object.keys`方法。

```
var o = {
  key1: 1,
  key2: 2
};

Object.keys(o);
// ['key1', 'key2']
```
`Object.keys`方法和`Object.getOwnPropertyNames`方法很相似，一般用来遍历对象的属性。它们的参数都是一个对象，都返回一个数组，该数组的成员都是对象自身的（而不是继承的）所有属性名。它们的区别在于，`Object.keys`方法只返回可枚举的属性（关于可枚举性的详细解释见后文），`Object.getOwnPropertyNames`方法还返回不可枚举的属性名。

```
var a = ["Hello", "World"];

Object.keys(a)
// ["0", "1"]

Object.getOwnPropertyNames(a)
// ["0", "1", "length"]
```

数组的length属性是不可枚举的属性，所以只出现在Object.getOwnPropertyNames方法的返回结果中。

一般情况下，几乎总是使用`Object.keys`方法，遍历数组的属性。

### delete命令

`delete`命令用于删除对象的属性，删除成功后返回`true`。

```
var o = {p: 1};
Object.keys(o) // ["p"]

delete o.p // true
o.p // undefined
Object.keys(o) // []
```

上面代码中，`delete`命令删除`o`对象的`p`属性。删除后，再读取`p`属性就会返回`undefined`，而且`Object.keys`方法的返回值中，`o`对象也不再包括该属性。

注意，删除一个不存在的属性，`delete`不报错，而且返回`true`。

```
var o = {};
delete o.p // true
```

上面代码中，`o`对象并没有`p`属性，但是`delete`命令照样返回`true`。因此，不能根据`delete`命令的结果，认定某个属性是存在的，只能保证读取这个属性肯定得到`undefined`。

只有一种情况，`delete`命令会返回`false`，那就是该属性存在，且不得删除。

```
var o = Object.defineProperty({}, 'p', {
  value: 123,
  configurable: false
});

o.p // 123
delete o.p // false
```

上面代码之中，`o`对象的`p`属性是不能删除的，所以`delete`命令返回`false`。

另外，需要注意的是，`delete`命令只能删除对象本身的属性，无法删除继承的属性。

```
var o = {};
delete o.toString // true
o.toString // function toString() { [native code] }
```

上面代码中，`toString`是对象`o`继承的属性，虽然`delete`命令返回`true`，但该属性并没有被删除，依然存在。

最后，`delete`命令不能删除`var`命令声明的变量，只能用来删除属性。

```
var p = 1;
delete p // false
delete window.p // false
```

### in运算符

`in`运算符用于检查对象是否包含某个属性（注意，检查的是键名，不是键值），如果包含就返回`true`，否则返回`false`。

```
var o = { p: 1 };
'p' in o // true
```

在JavaScript语言中，所有全局变量都是顶层对象（浏览器的顶层对象就是`window`对象）的属性，因此可以用`in`运算符判断，一个全局变量是否存在。

```
// 假设变量x未定义

// 写法一：报错
if (x) { return 1; }

// 写法二：不正确
if (window.x) { return 1; }

// 写法三：正确
if ('x' in window) { return 1; }
```

上面三种写法之中，如果`x`不存在，第一种写法会报错；如果`x`的值对应布尔值`false`（比如`x`等于空字符串），第二种写法无法得到正确结果；只有第三种写法，才能正确判断变量`x`是否存在。

`in`运算符的一个问题是，它不能识别对象继承的属性。

```
var o = new Object();
o.hasOwnProperty('toString') // false

'toString' in o // true
```

上面代码中，`toString`方法不是对象`o`自身的属性，而是继承的属性，`hasOwnProperty`方法可以说明这一点。但是，`in`运算符不能识别，对继承的属性也返回`true`。

### for...in循环

`for...in`循环用来遍历一个对象的全部属性。

```
var o = {a: 1, b: 2, c: 3};

for (var i in o) {
  console.log(o[i]);
}
// 1
// 2
// 3
```

`for...in`循环有两个使用注意点。

- 它遍历的是对象所有可遍历（enumerable）的属性，会跳过不可遍历的属性
- 它不仅遍历对象自身的属性，还遍历继承的属性。

请看下面的例子。

```
// name 是 Person 本身的属性
function Person(name) {
  this.name = name;
}

// describe是Person.prototype的属性
Person.prototype.describe = function () {
  return 'Name: '+this.name;
};

var person = new Person('Jane');

// for...in循环会遍历实例自身的属性（name），
// 以及继承的属性（describe）
for (var key in person) {
  console.log(key);
}
// name
// describe
```

上面代码中，`name`是对象本身的属性，`describe`是对象继承的属性，`for...in`循环的遍历会包括这两者。

如果只想遍历对象本身的属性，可以使用`hasOwnProperty`方法，在循环内部判断一下是不是自身的属性。

```
for (var key in person) {
  if (person.hasOwnProperty(key)) {
    console.log(key);
  }
}
// name
```

对象`person`其实还有其他继承的属性，比如`toString`。

```
person.toString()
// "[object Object]"
```

这个`toString`属性不会被`for...in`循环遍历到，因为它默认设置为“不可遍历”。

一般情况下，都是只想遍历对象自身的属性，所以不推荐使用`for...in`循环，可以使用`Object.keys`方法。

### with语句

`with`语句的格式如下：

```
with (object) {
  statements;
}
```

它的作用是操作同一个对象的多个属性时，提供一些书写的方便。

```
// 例一
with (o) {
  p1 = 1;
  p2 = 2;
}
// 等同于
o.p1 = 1;
o.p2 = 2;

// 例二
with (document.links[0]){
  console.log(href);
  console.log(title);
  console.log(style);
}
// 等同于
console.log(document.links[0].href);
console.log(document.links[0].title);
console.log(document.links[0].style);
```
## 数组

数组（array）是按次序排列的一组值。每个值的位置都有编号（从0开始），整个数组用方括号表示。

```
var arr = ['a', 'b', 'c'];
```
本质上，数组属于一种特殊的对象。`typeof`运算符会返回数组的类型是`object`。

```
typeof [1, 2, 3] // "object"
```
数组的特殊性体现在，它的键名是按次序排列的一组整数（0，1，2...）。

```
var arr = ['a', 'b', 'c'];

Object.keys(arr)
// ["0", "1", "2"]
```
JavaScript语言规定，对象的键名一律为字符串，所以，数组的键名其实也是字符串。之所以可以用数值读取，是因为非字符串的键名会被转为字符串。

### 定义

数组的生成方法，通常有二种方法。

```
var arr1 = [];
var arr2 = new Array();
```

除了在定义时赋值，数组也可以先定义后赋值。

```
var arr = [];

arr[0] = 'a';
arr[1] = 'b';
arr[2] = 'c';
```

任何类型的数据，都可以放入数组。

```
var arr = [
  {a: 1},
  [1, 2, 3],
  function() {return true;}
];

arr[0] // Object {a: 1}
arr[1] // [1, 2, 3]
arr[2] // function (){return true;}
```
如果数组的元素还是数组，就形成了多维数组。

```
var a = [[1, 2], [3, 4]];
a[0][1] // 2
a[1][1] // 4
```
### length属性

数组的`length`属性，返回数组的成员数量。

```
['a', 'b', 'c'].length // 3
```
JavaScript使用一个32位整数，保存数组的元素个数。这意味着，数组成员最多只有4294967295个（2<sup>32</sup> - 1）个，也就是说`length`属性的最大值就是4294967295。

只要是数组，就一定有`length`属性。该属性是一个动态的值，等于键名中的最大整数加上`1`。

```
var arr = ['a', 'b'];
arr.length // 2

arr[2] = 'c';
arr.length // 3

arr[9] = 'd';
arr.length // 10

arr[1000] = 'e';
arr.length // 1001
```

上面代码表示，数组的数字键不需要连续，`length`属性的值总是比最大的那个整数键大`1`。另外，这也表明数组是一种动态的数据结构，可以随时增减数组的成员。

`length`属性是可写的。如果人为设置一个小于当前成员个数的值，该数组的成员会自动减少到`length`设置的值。

```
var arr = [ 'a', 'b', 'c' ];
arr.length // 3

arr.length = 2;
arr // ["a", "b"]
```

上面代码表示，当数组的`length`属性设为2（即最大的整数键只能是1）那么整数键2（值为`c`）就已经不在数组中了，被自动删除了。

==将数组清空的一个有效方法，就是将`length`属性设为0。==

```
var arr = [ 'a', 'b', 'c' ];

arr.length = 0;
arr // []
```

如果人为设置`length`大于当前元素个数，则数组的成员数量会增加到这个值，新增的位置都是空位。

```
var a = ['a'];

a.length = 3;
a[1] // undefined
```

上面代码表示，当`length`属性设为大于数组个数时，读取新增的位置都会返回`undefined`。

如果人为设置`length`为不合法的值，JavaScript会报错。

```
// 设置负值
[].length = -1
// RangeError: Invalid array length

// 数组元素个数大于等于2的32次方
[].length = Math.pow(2, 32)
// RangeError: Invalid array length

// 设置字符串
[].length = 'abc'
// RangeError: Invalid array length
```

值得注意的是，由于数组本质上是对象的一种，所以我们可以为数组添加属性，但是这不影响`length`属性的值。

```
var a = [];

a['p'] = 'abc';
a.length // 0

a[2.1] = 'abc';
a.length // 0
```

上面代码将数组的键分别设为字符串和小数，结果都不影响`length`属性。因为，`length`属性的值就是等于最大的数字键加1，而这个数组没有整数键，所以`length`属性保持为0。

### 类似数组的对象

在JavaScript中，有些对象被称为“类似数组的对象”（array-like object）。意思是，它们看上去很像数组，可以使用`length`属性，但是它们并不是数组，所以无法使用一些数组的方法。

下面就是一个类似数组的对象。

```
var obj = {
  0: 'a',
  1: 'b',
  2: 'c',
  length: 3
};

obj[0] // 'a'
obj[2] // 'c'
obj.length // 3
obj.push('d') // TypeError: obj.push is not a function
```

上面代码中，变量`obj`是一个对象，使用的时候看上去跟数组很像，但是无法使用数组的方法。这就是类似数组的对象。

类似数组的对象只有一个特征，就是具有`length`属性。换句话说，只要有`length`属性，就可以认为这个对象类似于数组。但是，对象的`length`属性不是动态值，不会随着成员的变化而变化。

典型的类似数组的对象是函数的`arguments`对象，以及大多数DOM元素集，还有字符串。

```
// arguments对象
function args() { return arguments }
var arrayLike = args('a', 'b');

arrayLike[0] // 'a'
arrayLike.length // 2
arrayLike instanceof Array // false

// DOM元素集
var elts = document.getElementsByTagName('h3');
elts.length // 3
elts instanceof Array // false

// 字符串
'abc'[1] // 'b'
'abc'.length // 3
'abc' instanceof Array // false
```

数组的`slice`方法将类似数组的对象，变成真正的数组。

```
var arr = Array.prototype.slice.call(arrayLike);
```
### in 运算符

检查某个键名是否存在的运算符`in`，适用于对象，也适用于数组。

```
var arr = [ 'a', 'b', 'c' ];
2 in arr  // true
'2' in arr // true
4 in arr // false
```

上面代码表明，数组存在键名为`2`的键。由于键名都是字符串，所以数值`2`会自动转成字符串。

注意，如果数组的某个位置是空位，`in`运算符返回`false`。

```
var arr = [];
arr[100] = 'a';

100 in arr // true
1 in arr // false
```

上面代码中，数组`arr`只有一个成员`arr[100]`，其他位置的键名都会返回`false`。

### for...in 循环和数组的遍历

`for...in`循环不仅可以遍历对象，也可以遍历数组，毕竟数组只是一种特殊对象。

```
var a = [1, 2, 3];

for (var i in a) {
  console.log(a[i]);
}
// 1
// 2
// 3
```

但是，`for...in`不仅会遍历数组所有的数字键，还会遍历非数字键。

```
var a = [1, 2, 3];
a.foo = true;

for (var key in a) {
  console.log(key);
}
// 0
// 1
// 2
// foo
```

上面代码在遍历数组时，也遍历到了非整数键`foo`。所以，不推荐使用`for...in`遍历数组。

数组的遍历可以考虑使用`for`循环或`while`循环。

```
var a = [1, 2, 3];

// for循环
for(var i = 0; i < a.length; i++) {
  console.log(a[i]);
}

// while循环
var i = 0;
while (i < a.length) {
  console.log(a[i]);
  i++;
}

var l = a.length;
while (l--) {
  console.log(a[l]);
}
```

上面代码是三种遍历数组的写法。最后一种写法是逆向遍历，即从最后一个元素向第一个元素遍历。

数组的`forEach`方法，也可以用来遍历数组。

```
var colors = ['red', 'green', 'blue'];
colors.forEach(function (color) {
  console.log(color);
});
```
### 静态方法 

- Array.isArray()

`Array.isArray`方法用来判断一个值是否为数组。它可以弥补`typeof`运算符的不足。

```
var a = [1, 2, 3];

typeof a // "object"
Array.isArray(a) // true
```
### 实例方法

- valueOf()  返回数组本身。

  ```
  var a = [1, 2, 3];
  a.valueOf() // [1, 2, 3]
  ```

- toString() 返回数组的字符串形式。

  ```
  var a = [1, 2, 3];
  a.toString() // "1,2,3"

  var a = [1, 2, 3, [4, 5, 6]];
  a.toString() // "1,2,3,4,5,6"
  ```

- push()

  `push`方法用于在数组的末端添加一个或多个元素，并返回添加新元素后的数组长度。注意，该方法会改变原数组。

- pop()

  `pop`方法用于删除数组的最后一个元素，并返回该元素。注意，该方法会改变原数组。

  对空数组使用`pop`方法，不会报错，而是返回`undefined`。

  `push`和`pop`结合使用，就构成了“后进先出”的栈结构（stack）。

- join()

  `join`方法以参数作为分隔符，将所有数组成员组成一个字符串返回。如果不提供参数，默认用逗号分隔。

  如果数组成员是`undefined`或`null`或空位，会被转成空字符串。

- concat()

  `concat`方法用于多个数组的合并。它将新数组的成员，添加到原数组的尾部，然后返回一个新数组，原数组不变。

- shift()

  `shift`方法用于删除数组的第一个元素，并返回该元素。注意，该方法会改变原数组。

  `shift`方法可以遍历并清空一个数组。

  `push`和`shift`结合使用，就构成了“先进先出”的队列结构（queue）。

- unshift()

  `unshift`方法用于在数组的第一个位置添加元素，并返回添加新元素后的数组长度。注意，该方法会改变原数组。

- slice()

  `slice`方法用于提取原数组的一部分，返回一个新数组，原数组不变。

  ```
  // 格式
  arr.slice(start_index, upto_index);

  // 用法
  var a = ['a', 'b', 'c'];

  a.slice(0) // ["a", "b", "c"]
  a.slice(1) // ["b", "c"]
  a.slice(1, 2) // ["b"]
  a.slice(2, 6) // ["c"]
  a.slice() // ["a", "b", "c"]
  ```
  它的第一个参数为起始位置（从0开始），第二个参数为终止位置（但该位置的元素本身不包括在内）。如果省略第二个参数，则一直返回到原数组的最后一个成员。

- forEach()

  `forEach`方法与`map`方法很相似，也是遍历数组的所有成员，执行某种操作，但是`forEach`方法一般不返回值，只用来操作数据。如果需要有返回值，一般使用`map`方法。

  `forEach`方法可以接受第二个参数，用来绑定回调函数的`this`关键字。

- indexOf()

  `indexOf`方法返回给定元素在数组中第一次出现的位置，如果没有出现则返回`-1`。

  `indexOf`方法还可以接受第二个参数，表示搜索的开始位置。

- lastIndexOf()

  `lastIndexOf`方法返回给定元素在数组中最后一次出现的位置，如果没有出现则返回`-1`。


## 函数

函数就是一段可以反复调用的代码块。函数还能接受输入的参数，不同的参数会返回不同的值。

JavaScript语言将函数看作一种值，与其它值（数值、字符串、布尔值等等）地位相同。凡是可以使用值的地方，就能使用函数。比如，可以把函数赋值给变量和对象的属性，也可以当作参数传入其他函数，或者作为函数的结果返回。函数只是一个可以执行的值，此外并无特殊之处。

```
function add(x, y) {
  return x + y;
}

// 将函数赋值给一个变量
var operator = add;

// 将函数作为参数和返回值
function a(op){
  return op;
}
a(add)(1, 1)
// 2
```
### 定义

JavaScript有三种方法，可以声明一个函数。

**（1）function命令**

`function`命令声明的代码区块，就是一个函数。`function`命令后面是函数名，函数名后面是一对圆括号，里面是传入函数的参数。函数体放在大括号里面。

```
function print(s) {
  console.log(s);
}
```

**（2）函数表达式**

除了用`function`命令声明函数，还可以采用变量赋值的写法。

```
var print = function(s) {
  console.log(s);
};
```

这种写法将一个匿名函数赋值给变量。这时，这个匿名函数又称函数表达式（Function Expression），因为赋值语句的等号右侧只能放表达式。

**（3）Function构造函数**

还有第三种声明函数的方式：`Function`构造函数。

```
var add = new Function(
  'x',
  'y',
  'return x + y'
);

// 等同于

function add(x, y) {
  return x + y;
}
```

你可以传递任意数量的参数给`Function`构造函数，只有最后一个参数会被当做函数体，如果只有一个参数，该参数就是函数体。

```
var foo = new Function(
  'return "hello world"'
);

// 等同于

function foo() {
  return 'hello world';
}
```

`Function`构造函数可以不使用`new`命令，返回结果完全一样。

总的来说，这种声明函数的方式非常不直观，几乎无人使用。

### 函数的重复声明

如果同一个函数被多次声明，后面的声明就会覆盖前面的声明。

```
function f() {
  console.log(1);
}
f() // 2

function f() {
  console.log(2);
}
f() // 2
```

上面代码中，后一次的函数声明覆盖了前面一次。而且，由于函数名的提升（参见下文），前一次声明在任何时候都是无效的，这一点要特别注意。

### 函数名的提升

JavaScript引擎将函数名视同变量名，所以采用`function`命令声明函数时，整个函数会像变量声明一样，被提升到代码头部。所以，下面的代码不会报错。

```
f();

function f() {}
```

表面上，上面代码好像在声明之前就调用了函数`f`。但是实际上，由于“变量提升”，函数`f`被提升到了代码头部，也就是在调用之前已经声明了。但是，如果采用赋值语句定义函数，JavaScript就会报错。

```
f();
var f = function (){};
// TypeError: undefined is not a function
```

上面的代码等同于下面的形式。

```
var f;
f();
f = function () {};
```

上面代码第二行，调用`f`的时候，`f`只是被声明了，还没有被赋值，等于`undefined`，所以会报错。因此，如果同时采用`function`命令和赋值语句声明同一个函数，最后总是采用赋值语句的定义。

```
var f = function() {
  console.log('1');
}

function f() {
  console.log('2');
}

f() // 1
```
### 属性和方法

**（1）name属性**

  返回紧跟在`function`关键字之后的那个函数名。

**（2）length属性**

  返回函数预期传入的参数个数，即函数定义之中的参数个数。

**（3）toString方法**

  返回函数的源码。

### 作用域

作用域（scope）指的是变量存在的范围。Javascript只有两种作用域：一种是全局作用域，变量在整个程序中一直存在，所有地方都可以读取；另一种是函数作用域，变量只在函数内部存在。

在函数外部声明的变量就是全局变量（global variable），它可以在函数内部读取。

在函数内部定义的变量，外部无法读取，称为“局部变量”（local variable）。

函数内部定义的变量，会在该作用域内覆盖同名全局变量。

对于`var`命令来说，局部变量只能在函数内部声明，在其他区块中声明，一律都是全局变量。

### 函数内部的变量提升

与全局作用域一样，函数作用域内部也会产生“变量提升”现象。`var`命令声明的变量，不管在什么位置，变量声明都会被提升到函数体的头部。

```
function foo(x) {
  if (x > 100) {
    var tmp = x - 100;
  }
}
```

上面的代码等同于

```
function foo(x) {
  var tmp;
  if (x > 100) {
    tmp = x - 100;
  };
}
```
### 函数本身的作用域

函数本身也是一个值，也有自己的作用域。它的作用域与变量一样，就是其声明时所在的作用域，与其运行时所在的作用域无关。

```
var a = 1;
var x = function () {
  console.log(a);
};

function f() {
  var a = 2;
  x();
}

f() // 1
```

上面代码中，函数`x`是在函数`f`的外部声明的，所以它的作用域绑定外层，内部变量`a`不会到函数`f`体内取值，所以输出`1`，而不是`2`。

总之，函数执行时所在的作用域，是定义时的作用域，而不是调用时所在的作用域。

正是这种机制，构成了下文要讲解的“闭包”现象。

### 参数

函数运行的时候，有时需要提供外部数据，不同的外部数据会得到不同的结果，这种外部数据就叫参数。

**（1）参数的省略**

函数参数不是必需的，Javascript允许省略参数。

```
function f(a, b) {
  return a;
}

f(1, 2, 3) // 1
f(1) // 1
f() // undefined

f.length // 2
```

上面代码的函数`f`定义了两个参数，但是运行时无论提供多少个参数（或者不提供参数），JavaScript都不会报错，被省略的参数的值就变为`undefined`。

**（2）参数的默认值**

```
function f(a) {
  (a !== undefined && a !== null) ? a = a : a = 1;
  return a;
}

f() // 1
f('') // ""
f(0) // 0
```
**（3）传递方式**

函数参数如果是原始类型的值（数值、字符串、布尔值），传递方式是传值传递（passes by value）。这意味着，在函数体内修改参数值，不会影响到函数外部。

```
var p = 2;

function f(p) {
  p = 3;
}
f(p);

p // 2
```

如果函数参数是复合类型的值（数组、对象、其他函数），传递方式是传址传递（pass by reference）。也就是说，传入函数的原始值的地址，因此在函数内部修改参数，将会影响到原始值。

```
var obj = {p: 1};

function f(o) {
  o.p = 2;
}
f(obj);

obj.p // 2
```

**（4）同名参数**

如果有同名的参数，则取最后出现的那个值。

```
function f(a, a) {
  console.log(a);
}

f(1, 2) // 2

function ff(a, a){
  console.log(a);
}

ff(1) // undefined
```

**（5）arguments对象**

由于JavaScript允许函数有不定数目的参数，所以我们需要一种机制，可以在函数体内部读取所有参数。这就是`arguments`对象的由来。

`arguments`对象包含了函数运行时的所有参数，`arguments[0]`就是第一个参数，`arguments[1]`就是第二个参数，以此类推。这个对象只有在函数体内部，才可以使用。

```
var f = function(one) {
  console.log(arguments[0]);
  console.log(arguments[1]);
  console.log(arguments[2]);
}

f(1, 2, 3)
// 1
// 2
// 3
```
可以通过`arguments`对象的`length`属性，判断函数调用时到底带几个参数。

```
function f() {
  return arguments.length;
}

f(1, 2, 3) // 3
f(1) // 1
f() // 0
```
### 闭包

闭包（closure）是Javascript语言的一个难点，也是它的特色，很多高级应用都要依靠闭包实现。

要理解闭包，首先必须理解变量作用域。

在函数外部无法读取函数内部声明的变量。

```
function f1() {
  var n = 999;
}

console.log(n)
// Uncaught ReferenceError: n is not defined
```

上面代码中，函数`f1`内部声明的变量`n`，函数外是无法读取的。

如果出于种种原因，需要得到函数内的局部变量。正常情况下，这是办不到的，只有通过变通方法才能实现。那就是在函数的内部，再定义一个函数。

```
function f1() {
  var n = 999;
  function f2() {
　　console.log(n); // 999
  }
}
```
这就是JavaScript语言特有的"链式作用域"结构（chain scope），子对象会一级一级地向上寻找所有父对象的变量。所以，父对象的所有变量，对子对象都是可见的，反之则不成立。

既然`f2`可以读取`f1`的局部变量，那么只要把`f2`作为返回值，我们不就可以在`f1`外部读取它的内部变量了吗！

```
function f1() {
  var n = 999;
  function f2() {
    console.log(n);
  }
  return f2;
}

var result = f1();
result(); // 999
```
闭包就是函数`f2`，即能够读取其他函数内部变量的函数。由于在JavaScript语言中，只有函数内部的子函数才能读取内部变量，因此可以把闭包简单理解成“定义在一个函数内部的函数”。闭包最大的特点，就是它可以“记住”诞生的环境，比如`f2`记住了它诞生的环境`f1`，所以从`f2`可以得到`f1`的内部变量。在本质上，闭包就是将函数内部和函数外部连接起来的一座桥梁。

闭包的最大用处有两个，一个是可以读取函数内部的变量，另一个就是让这些变量始终保持在内存中，即闭包可以使得它诞生环境一直存在。请看下面的例子，闭包使得内部变量记住上一次调用时的运算结果。

```
function createIncrementor(start) {
  return function () {
    return start++;
  };
}

var inc = createIncrementor(5);

inc() // 5
inc() // 6
inc() // 7
```

上面代码中，`start`是函数`createIncrementor`的内部变量。通过闭包，`start`的状态被保留了，每一次调用都是在上一次调用的基础上进行计算。从中可以看到，闭包`inc`使得函数`createIncrementor`的内部环境，一直存在。所以，闭包可以看作是函数内部作用域的一个接口。

为什么会这样呢？原因就在于`inc`始终在内存中，而`inc`的存在依赖于`createIncrementor`，因此也始终在内存中，不会在调用结束后，被垃圾回收机制回收。

闭包的另一个用处，是封装对象的私有属性和私有方法。

```
function Person(name) {
  var _age;
  function setAge(n) {
    _age = n;
  }
  function getAge() {
    return _age;
  }

  return {
    name: name,
    getAge: getAge,
    setAge: setAge
  };
}

var p1 = Person('张三');
p1.setAge(25);
p1.getAge() // 25
```

上面代码中，函数`Person`的内部变量`_age`，通过闭包`getAge`和`setAge`，变成了返回对象`p1`的私有变量。

注意，外层函数每次运行，都会生成一个新的闭包，而这个闭包又会保留外层函数的内部变量，所以内存消耗很大。因此不能滥用闭包，否则会造成网页的性能问题。

```
//代码有什么问题,使用闭包来解决
for(var i=0; i<5; i++){
        $('<div>Print ' + i + '</div>')
        .click(function(){
            console.log(i);
        }).insertBefore('body');
    }
```

### 立即调用的函数表达式

在Javascript中，一对圆括号`()`是一种运算符，跟在函数名之后，表示调用该函数。比如，`print()`就表示调用`print`函数。

有时，我们需要在定义函数之后，立即调用该函数。这时，你不能在函数的定义之后加上圆括号，这会产生语法错误。

```
function(){ /* code */ }();
// SyntaxError: Unexpected token (
```

产生这个错误的原因是，`function`这个关键字即可以当作语句，也可以当作表达式。

为了避免解析上的歧义，JavaScript引擎规定，如果`function`关键字出现在行首，一律解释成语句。因此，JavaScript引擎看到行首是`function`关键字之后，认为这一段都是函数的定义，不应该以圆括号结尾，所以就报错了。

解决方法就是不要让`function`出现在行首，让引擎将其理解成一个表达式。最简单的处理，就是将其放在一个圆括号里面。

```
(function(){
