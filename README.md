# **Проект "Интерактивная модель"**

## Управление

- Дередвижение камеры осуществлется при зажатии колёсика мыши
- Для кругового вращения вокруг объекта необходимо:
  1) Взять объект в фокус (нажать левой кнопкой мыши на желаемый объект)
  2) Зажать правую кнопку мыши и вести мышь в необходимую сторону
- Приближение или отдвление реализовано на колёсико мыши

## Дополнительная информация

- Меню управления объектами по логике своей работы полностью повторяет данное в примере

![Menu Example](MenuExample.png)

- Прошу заметить, что в данном случае скрыть объект и изменить прозрачность не одно и то же. 
Так как это не обговорено в задании я решил дать им разные реализации 
(на уровне кода скрытие объекта работает при помощи оключения компонента renderer,
а измнение прозрачности работает с alpha каналом материалов)

- Компоненты добавляются в меню самостоятельно, для этого необходимо лишь
повесить на них скрипт и передать в него ControllableObjectInteractor (можно было бы искать при помощи рефлексии сцены,
но я решил что это не очень хорошая идея ввиду неявности компонентов необходимых скрипту для работы)