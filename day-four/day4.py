
import re

with open("input.txt") as f:
    lines = f.readlines()


string_split_re = re.compile("\s+")

class Card(object):

    def __init__(self, line):
        colon_pos = line.find(": ")

        self._game_number = int(line[len("Card "):colon_pos])

        pipe_pos = line.find("|", colon_pos)

        winning_num_string = line[colon_pos+2:pipe_pos].strip()
        self._winning_nums = set(string_split_re.split(winning_num_string))

        drawn_num_string = line[pipe_pos+1:].strip()
        self._drawn_nums = set(string_split_re.split(drawn_num_string))

        self._wins = self._winning_nums.intersection(self._drawn_nums)

        self.copies = 1

    @property
    def win_count(self):
        return len(self._wins)
    
    @property
    def copy_count(self):
        return self.copies

    def get_card_name(self):
        return "Card {number}".format(number=self._game_number)

    def copy_card(self, copies_to_make=1):
        self.copies += copies_to_make


sum = 0

cards = []

for line in lines:
    card = Card(line)

    cards.append(card)

    win_count = card.win_count
    if (win_count == 0):
        continue

    score = 1
    for i in range(win_count):
        if i < win_count-1:
            score *= 2

    sum += score

print("Solution 1: " + str(sum))


for i, card in enumerate(cards):
    win_count = card.win_count

    for offset in range(1, win_count+1):
        idx = i + offset
        if idx >= len(cards):
            break

        copy_card = cards[idx]
        copy_card.copy_card(card.copy_count)

sum = 0
for card in cards:
    sum += card.copy_count

print("Solution 2: " + str(sum))