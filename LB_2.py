class givonhie:
    def __init__(self, name, carstvo, otrad):
        self.name = name
        self.carstvo = carstvo
        self.otrad = otrad

    def sw(self):
        print(f"{self.name}")
class vild(givonhie):
    def __init__(self, name, carstvo, otrad, vid):
        super().__init__(name, carstvo, otrad)
        self.vid = vid
    
    def vv(self):
        print(f"Имя: {self.name} | Царство: {self.carstvo} | Отряд: {self.otrad} | Вид: {self.vid}")

stepka = vild("Бурый Медведь", "Животные", "Хищные", "Бурый медведь")

stepka.sw()
stepka.vv()